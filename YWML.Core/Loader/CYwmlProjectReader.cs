using Newtonsoft.Json;
using YWML.Src.Loader.DataClasses;

namespace YWML.Src.Loader
{
    public static class CYwmlProjectReader
    {
        private const string CONFIG_FILE = "ywml.json";

        public static CYwmlProject Read(string projectPath)
        {
            var configPath = FindConfigFile(projectPath);
            if (configPath == null)
            {
                throw new FileNotFoundException($"{CONFIG_FILE} was not found in the project.");
            }

            CYwmlProject? project;
            try
            {
                project = JsonConvert.DeserializeObject<CYwmlProject>(File.ReadAllText(configPath));
            }
            catch (JsonException)
            {
                throw new InvalidDataException("Wrong json format");
            }

            if (project == null)
            {
                throw new InvalidDataException("Wrong properties");
            }

            return project;
        }

        private static string? FindConfigFile(string projectPath)
        {
            if (!Directory.Exists(projectPath))
            {
                return null;
            }

            return Directory.EnumerateFiles(projectPath)
                .FirstOrDefault(file => Path.GetFileName(file).Equals(CONFIG_FILE, StringComparison.OrdinalIgnoreCase));
        }
    }
}
