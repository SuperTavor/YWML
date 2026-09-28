using global::Android.App;
using global::Android.Content;
using Microsoft.Maui.ApplicationModel;

namespace YWML.Android.Services
{
    /// <summary>
    /// Picks a folder via the Storage Access Framework (ACTION_OPEN_DOCUMENT_TREE).
    /// </summary>
    public static class CAndroidFolderPicker
    {
        private const int REQUEST_CODE = 0x594D; // 'YM'
        private static TaskCompletionSource<string?>? _tcs;

        public static Task<string?> PickAsync()
        {
            _tcs = new TaskCompletionSource<string?>();

            var activity = Platform.CurrentActivity!;
            var intent = new Intent(Intent.ActionOpenDocumentTree);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission | ActivityFlags.GrantPersistableUriPermission);
            activity.StartActivityForResult(intent, REQUEST_CODE);

            return _tcs.Task;
        }

        public static void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            if (requestCode != REQUEST_CODE)
            {
                return;
            }

            if (resultCode == Result.Ok && data?.Data != null)
            {
                var uri = data.Data;
                var takeFlags = data.Flags & (ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);
                Platform.CurrentActivity!.ContentResolver!.TakePersistableUriPermission(uri, takeFlags);
                _tcs?.TrySetResult(uri.ToString());
            }
            else
            {
                _tcs?.TrySetResult(null);
            }
        }
    }
}
