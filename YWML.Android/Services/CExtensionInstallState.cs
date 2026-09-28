namespace YWML.Android.Services
{
    public sealed class CExtensionInstallState
    {
        public event Action? Changed;

        public bool IsBusy { get; private set; }
        public int Percent { get; private set; }
        public string Status { get; private set; } = string.Empty;

        public void Start(string status)
        {
            IsBusy = true;
            Percent = 0;
            Status = status;
            Changed?.Invoke();
        }

        public void SetStatus(string status)
        {
            Status = status;
            Changed?.Invoke();
        }

        public void SetPercent(int percent)
        {
            Percent = percent;
            Changed?.Invoke();
        }

        public void Finish()
        {
            IsBusy = false;
            Percent = 0;
            Status = string.Empty;
            Changed?.Invoke();
        }
    }
}
