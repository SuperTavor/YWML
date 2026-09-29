namespace YWML.Src.Warnings.DataClasses
{
    public sealed class SModWarning
    {
        public SModWarning(string title, string message)
        {
            Title = title;
            Message = message;
        }

        public string Title { get; }
        public string Message { get; }
    }
}
