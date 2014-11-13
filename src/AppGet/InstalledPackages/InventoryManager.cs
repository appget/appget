using System;
using System.Collections.Generic;
using System.Linq;
using AppGet.FileSystem;
using AppGet.HostSystem;
using AppGet.Serialization;

namespace AppGet.InstalledPackages
{
    public interface IInventoryManager
    {
        List<InstalledPackage> GetInstalledPackages();
        List<InstalledPackage> GetInstalledPackages(string id);
        void AddPackage(InstalledPackage installedPackage);
        void RemovePackage(InstalledPackage installedPackage);
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

        public List<InstalledPackage> GetInstalledPackages()
        {
            var packageListPath = _pathResolver.InstalledPackageList;

            if (!_fileSystem.FileExists(packageListPath))
            {
                return new List<InstalledPackage>();
            }

            var content = _fileSystem.ReadAllText(packageListPath);
            return Yaml.Deserialize<List<InstalledPackage>>(content);
        }

        public List<InstalledPackage> GetInstalledPackages(string id)
        {
            return GetInstalledPackages().Where(p => p.Id == id).ToList();
        }

        public void AddPackage(InstalledPackage installedPackage)
        {
            var currentPackages = GetInstalledPackages();
            currentPackages.Add(installedPackage);
            WritePackageList(currentPackages);
        }

        public void RemovePackage(InstalledPackage installedPackage)
        {
            var currentPackages = GetInstalledPackages();
            var updatedList = currentPackages.Where(c => !(c.Id == installedPackage.Id && c.Version == installedPackage.Version));
            WritePackageList(updatedList);
        }

        public bool IsInstalled(string id)
        {
            return GetInstalledPackages().Any(c => String.Equals(c.Id, id, StringComparison.InvariantCultureIgnoreCase));
        }

        private void WritePackageList(IEnumerable<InstalledPackage> packages)
        {
            var yamlpackageListPath = _pathResolver.InstalledPackageList;
            var yaml = Yaml.Serialize(packages);
            _fileSystem.WriteAllText(yamlpackageListPath, yaml);
        }
    }
}
