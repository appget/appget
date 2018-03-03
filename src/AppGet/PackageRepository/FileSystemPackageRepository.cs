using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public Task<PackageInfo> Get(string id, string tag)
        {
            if (string.IsNullOrWhiteSpace(_config.LocalRepository))
            {
                return null;
            }

            var pkgDir = Path.Combine(_config.LocalRepository, id);
            if (!_fileSystem.DirectoryExists(pkgDir))
            {
                return null;
            }

            var packages = _fileSystem.GetFiles(pkgDir, "*.yaml").Select(Read).Where(c => c.Tag == tag);
            return Task.FromResult(packages.OrderByDescending(c => c.Tag).FirstOrDefault());
        }

        private PackageInfo Read(string path)
        {
            var yaml = _fileSystem.ReadAllText(path);
            var manifest = Yaml.Deserialize<PackageManifest>(yaml);
            var fileName = Path.GetFileNameWithoutExtension(path);

            var indexOfTag = fileName.IndexOf(".");

            var tag = "";
            if (indexOfTag > 0)
            {
                tag = fileName.Substring(indexOfTag).Trim('.', ' ');
            }


            return new PackageInfo
            {
                Id = manifest.Id,
                Tag = string.IsNullOrWhiteSpace(tag) ? null : tag,
                ManifestPath = path
            };
        }

        public Task<List<PackageInfo>> Search(string term)
        {
            // TODO
            return Task.FromResult(new List<PackageInfo>());
        }
    }
}