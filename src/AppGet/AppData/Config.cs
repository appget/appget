using System;
using System.IO;
using AppGet.FileSystem;
using AppGet.HostSystem;
using AppGet.Manifest.Serialization;
using Newtonsoft.Json;

namespace AppGet.AppData
{
    public interface IConfig
    {
        string LocalRepository { get; }
        string ApiKey { get; }
    }

    public class Config : IConfig
    {
        public string LocalRepository { get; }
        public string ApiKey { get; }

        public Config(IFileSystem fileSystem, IPathResolver pathResolver)
        {
            var configFile = Path.Combine(pathResolver.AppDataDirectory, "config.json");
            LocalRepository = Path.Combine(pathResolver.AppDataDirectory, "Repository\\");

            if (fileSystem.FileExists(configFile))
            {
                var text = fileSystem.ReadAllText(configFile);
                var settings = Json.Deserialize<dynamic>(text);
                LocalRepository = settings.localRepository;
                ApiKey = settings.apiKey;
            }
            else
            {
                ApiKey = Guid.NewGuid().ToString().Replace("-", "");
                fileSystem.WriteAllText(configFile, Json.Serialize(this, Formatting.Indented));
            }
        }
    }
}