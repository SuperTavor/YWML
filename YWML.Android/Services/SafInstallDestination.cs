using global::Android.Content;
using global::Android.Provider;
using YWML.Src.Install;

namespace YWML.Android.Services
{
    /// <summary>
    /// Writes into a user-granted SAF document tree (the emulator "User folder"),
    /// using the base Android DocumentsContract APIs (no AndroidX dependency).
    /// </summary>
    public sealed class SafInstallDestination : IInstallDestination
    {
        private readonly string _treeUri;
        private readonly string _baseRelativePath;

        public SafInstallDestination(string treeUri, string baseRelativePath)
        {
            _treeUri = treeUri;
            _baseRelativePath = baseRelativePath;
        }

        public IInstallDestination Parent => new SafInstallDestination(_treeUri, GetParentRelativePath(_baseRelativePath));

        private static string GetParentRelativePath(string baseRelativePath)
        {
            var normalized = baseRelativePath.Replace('\\', '/').Trim('/');
            var lastSlash = normalized.LastIndexOf('/');
            return lastSlash < 0 ? string.Empty : normalized[..lastSlash];
        }

        public Stream OpenWrite(string relativePath)
        {
            var context = global::Android.App.Application.Context!;
            var resolver = context.ContentResolver!;
            var treeUri = global::Android.Net.Uri.Parse(_treeUri)!;

            var combined = string.IsNullOrEmpty(_baseRelativePath)
                ? relativePath
                : $"{_baseRelativePath}/{relativePath}";
            var segments = combined.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries);

            var parentDocId = DocumentsContract.GetTreeDocumentId(treeUri);
            for (var i = 0; i < segments.Length - 1; i++)
            {
                parentDocId = GetOrCreateDirectory(resolver, treeUri, parentDocId, segments[i]);
            }

            var fileUri = CreateFile(resolver, treeUri, parentDocId, segments[^1]);
            return resolver.OpenOutputStream(fileUri)!;
        }

        private static string GetOrCreateDirectory(ContentResolver resolver, global::Android.Net.Uri treeUri, string parentDocId, string name)
        {
            var existing = FindChild(resolver, treeUri, parentDocId, name, DocumentsContract.Document.MimeTypeDir);
            if (existing != null)
            {
                return DocumentsContract.GetDocumentId(existing)!;
            }

            var parentUri = DocumentsContract.BuildDocumentUriUsingTree(treeUri, parentDocId);
            var created = DocumentsContract.CreateDocument(resolver, parentUri, DocumentsContract.Document.MimeTypeDir, name)!;
            return DocumentsContract.GetDocumentId(created)!;
        }

        private static global::Android.Net.Uri CreateFile(ContentResolver resolver, global::Android.Net.Uri treeUri, string parentDocId, string name)
        {
            var existing = FindChild(resolver, treeUri, parentDocId, name, null);
            if (existing != null)
            {
                DocumentsContract.DeleteDocument(resolver, existing);
            }

            var parentUri = DocumentsContract.BuildDocumentUriUsingTree(treeUri, parentDocId);
            return DocumentsContract.CreateDocument(resolver, parentUri, "application/octet-stream", name)!;
        }

        private static global::Android.Net.Uri? FindChild(ContentResolver resolver, global::Android.Net.Uri treeUri, string parentDocId, string name, string? mimeType)
        {
            var childrenUri = DocumentsContract.BuildChildDocumentsUriUsingTree(treeUri, parentDocId);
            var projection = new[]
            {
                DocumentsContract.Document.ColumnDocumentId,
                DocumentsContract.Document.ColumnDisplayName,
                DocumentsContract.Document.ColumnMimeType
            };

            using var cursor = resolver.Query(childrenUri, projection, null, null, null);
            if (cursor == null)
            {
                return null;
            }

            while (cursor.MoveToNext())
            {
                var displayName = cursor.GetString(1);
                var mime = cursor.GetString(2);
                if (displayName != name || (mimeType != null && mime != mimeType))
                {
                    continue;
                }

                var documentId = cursor.GetString(0)!;
                return DocumentsContract.BuildDocumentUriUsingTree(treeUri, documentId);
            }

            return null;
        }
    }
}
