namespace YWML.Src.Forms
{
    public static class CUiUtils
    {
        public static string? ChooseFolder(string description)
        {
            var fbd = new FolderBrowserDialog
            {
                UseDescriptionForTitle = true,
                Description = description,
            };

            return fbd.ShowDialog() == DialogResult.OK ? fbd.SelectedPath : null;
        }
    }
}
