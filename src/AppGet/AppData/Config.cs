using System.IO;
using AppGet.FileSystem;
using AppGet.HostSystem;

namespace AppGet.AppData
{
    public class Config
    {
        public string LocalRepository { get; set; }
        public string ApiKey { get; set; }
    }

    public class ConfigStore : StoreBase<Config>
    {
        private readonly IPathResolver _pathResolver;

        //        public Config(IFileSystem fileSystem, IPathResolver pathResolver):base()
        //        {
        //            var configFile = Path.Combine(pathResolver.AppDataDirectory, "config.json");
        //            LocalRepository = Path.Combine(pathResolver.AppDataDirectory, "Repository\\");
        //
        //            if (fileSystem.FileExists(configFile))
        //            {
        //                var text = fileSystem.ReadAllText(configFile);
        //                var settings = Json.Deserialize<dynamic>(text);
        //                LocalRepository = settings.localRepository;
        //                ApiKey = settings.apiKey;
        //            }
        //            else
        //            {
        //                ApiKey = Guid.NewGuid().ToString().Replace("-", "");
        //                fileSystem.WriteAllText(configFile, Json.Serialize(this, Formatting.Indented));
        //            }
        //        }

        protected override string Name => "config";

        public ConfigStore(IFileSystem fileSystem, IPathResolver pathResolver)
            : base(fileSystem, pathResolver)
        {
            _pathResolver = pathResolver;
        }

        protected override void SetDefaults(Config data)
        {
            if (data.LocalRepository == null)
            {
                data.LocalRepository = Path.Combine(_pathResolver.AppDataDirectory, @"Repository\");
            }
        }
    }
}