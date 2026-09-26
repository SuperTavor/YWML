using Newtonsoft.Json;
using YWML.Src.Loader.DataClasses;

namespace YWML.Src.Loader
{
    public static class CYwmlProjectReader
    {
        public static CYwmlProject Read(string projectPath)
        {
            var configPath = Path.Combine(projectPath, "ywml.json");
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException("ywml.json was not found in the project.");
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
    }
}
