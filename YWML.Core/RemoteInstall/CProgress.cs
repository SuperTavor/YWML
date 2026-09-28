namespace YWML.Src.RemoteInstall
{
    internal sealed class CProgress<T> : IProgress<T>
    {
        private readonly Action<T> _callback;

        public CProgress(Action<T> callback)
        {
            _callback = callback;
        }

        public void Report(T value)
        {
            _callback(value);
        }
    }
}
