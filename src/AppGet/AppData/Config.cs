using System.ComponentModel;
using System.IO;
using AppGet.FileSystem;
using AppGet.HostSystem;

namespace AppGet.AppData
{
    public class Config
    {
        public string LocalRepository { get; set; }
        public string ApiKey { get; set; }

        [DefaultValue(true)]
        public bool? ShareAnonymousData { get; set; }
    }

    public class ConfigStore : StoreBase<Config>
    {
        private readonly IPathResolver _pathResolver;

        protected override string Name => "config";

        public ConfigStore(IFileSystem fileSystem, IPathResolver pathResolver)
            : base(fileSystem, pathResolver)
        {
            _pathResolver = pathResolver;
        }

        protected override void SetInitialValues(Config data)
        {
            if (data.LocalRepository == null)
            {
                data.LocalRepository = Path.Combine(_pathResolver.AppDataDirectory, @"Repository\");
            }
        }
    }
}