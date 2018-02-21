using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppGet.AppData;
using AppGet.FileSystem;
using AppGet.Manifests;
using AppGet.Serialization;

namespace AppGet.PackageRepository
{
    public class LocalPackageRepository : IPackageRepository
    {
        private readonly IFileSystem _fileSystem;
        private readonly IConfig _config;

        public LocalPackageRepository(IFileSystem fileSystem, IConfig config)
        {
            _fileSystem = fileSystem;
            _config = config;
        }

        public PackageInfo GetLatest(string name)
        {
            if (string.IsNullOrWhiteSpace(_config.LocalRepository))
            {
                return null;
            }

            var pkgDir = Path.Combine(_config.LocalRepository, name);
            if (!_fileSystem.DirectoryExists(pkgDir))
            {
                return null;
            }

            var packages = _fileSystem.GetFiles(pkgDir, "*.yaml").Select(Read);
            return packages.OrderByDescending(c => c.MajorVersion).FirstOrDefault();
        }

        private PackageInfo Read(string path)
        {
            var yaml = _fileSystem.ReadAllText(path);
            var manifest = Yaml.Deserialize<PackageManifest>(yaml);

            return new PackageInfo
            {
                Id = manifest.Id,
                MajorVersion = "latest",
                ManifestUrl = path
            };
        }

        public List<PackageInfo> Search(string term)
        {
            return new List<PackageInfo>();
        }
    }
}