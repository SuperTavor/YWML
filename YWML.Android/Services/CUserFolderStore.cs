namespace YWML.Android.Services
{
    public sealed class CUserFolderStore
    {
        private const string KEY = "emulator_user_folder_uri";

        public string? TreeUri => Preferences.Default.Get<string?>(KEY, null);

        public bool IsSet => !string.IsNullOrEmpty(TreeUri);

        public string? DisplayPath
        {
            get
            {
                var uri = TreeUri;
                if (string.IsNullOrEmpty(uri))
                {
                    return null;
                }

                try
                {
                    var treeUri = global::Android.Net.Uri.Parse(uri)!;
                    var documentId = global::Android.Provider.DocumentsContract.GetTreeDocumentId(treeUri);
                    if (string.IsNullOrEmpty(documentId))
                    {
                        return uri;
                    }

                    var separator = documentId.IndexOf(':');
                    if (separator < 0)
                    {
                        return documentId;
                    }

                    var volume = documentId[..separator];
                    var path = documentId[(separator + 1)..];
                    var root = volume == "primary" ? "/storage/emulated/0" : $"/storage/{volume}";
                    return $"{root}/{path}";
                }
                catch
                {
                    return uri;
                }
            }
        }

        public void Set(string treeUri) => Preferences.Default.Set(KEY, treeUri);

        public void Clear() => Preferences.Default.Remove(KEY);

        //Path inside the emulator's User folder where a game's modded ROMFS lives.
        public static string ModsRelativeRoot(string titleId) => $"load/mods/{titleId}/romfs";
    }
}
