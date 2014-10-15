using System.Collections.Generic;
using AppGet.FileSystem;
using AppGet.HostSystem;
using AppGet.PackageRepository;
using AppGet.Serialization;

namespace AppGet.InstalledPackages
{
    public interface IInventoryManager
    {
        List<PackageInfo> GetInstalledPackages();
        void AddInstalledPackage(PackageInfo packageInfo);
    }


    public class InventoryManager : IInventoryManager
    {
        private readonly IPathResolver _pathResolver;
        private readonly IFileSystem _fileSystem;

        public InventoryManager(IPathResolver pathResolver, IFileSystem fileSystem)
        {
            _pathResolver = pathResolver;
            _fileSystem = fileSystem;
        }

        public List<PackageInfo> GetInstalledPackages()
        {
            var packageListPath = _pathResolver.InstalledPackageList;

            if (!_fileSystem.FileExists(packageListPath))
            {
                return new List<PackageInfo>();
            }

            var content = _fileSystem.ReadAllText(packageListPath);
            return Yaml.Deserialize<List<PackageInfo>>(content);
        }

        public void AddInstalledPackage(PackageInfo packageInfo)
        {
            var yamlpackageListPath = _pathResolver.InstalledPackageList;

            var currentPackages = GetInstalledPackages();

            currentPackages.Add(packageInfo);

            var yaml = Yaml.Serialize(currentPackages);

            _fileSystem.WriteAllText(yamlpackageListPath, yaml);
        }
    }
}
