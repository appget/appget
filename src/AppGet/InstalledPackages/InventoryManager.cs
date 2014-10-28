using System;
using System.Collections.Generic;
using System.Linq;
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
        void RemoveInstalledPackage(PackageInfo packageInfo);
        bool IsInstalled(string id);
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
            var currentPackages = GetInstalledPackages();
            currentPackages.Add(packageInfo);
            WritePackageList(currentPackages);
        }

        public void RemoveInstalledPackage(PackageInfo packageInfo)
        {
            var currentPackages = GetInstalledPackages();
            var updatedList = currentPackages.Where(c => !(c.Id == packageInfo.Id && c.Version == packageInfo.Version));
            WritePackageList(updatedList);
        }

        public bool IsInstalled(string id)
        {
            return GetInstalledPackages().Any(c => String.Equals(c.Id, id, StringComparison.InvariantCultureIgnoreCase));
        }


        private void WritePackageList(IEnumerable<PackageInfo> packages)
        {
            var yamlpackageListPath = _pathResolver.InstalledPackageList;
            var yaml = Yaml.Serialize(packages);
            _fileSystem.WriteAllText(yamlpackageListPath, yaml);
        }
    }
}
