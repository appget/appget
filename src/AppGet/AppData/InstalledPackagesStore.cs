using System;
using System.Collections.Generic;
using System.Linq;
using AppGet.FileSystem;
using AppGet.HostSystem;
using AppGet.Infrastructure.Events;
using AppGet.Installers.Events;
using AppGet.Manifest;

namespace AppGet.AppData
{
    public class InstalledPackage
    {
        public string Id { get; set; }
        public string Tag { get; set; }
        public string Version { get; set; }
        public DateTime InstallDate { get; set; }
    }

    public class InstalledPackagesStore : StoreBase<List<InstalledPackage>>,
        IHandle<InstallationSuccessfulEvent>
    {
        public InstalledPackagesStore(IFileSystem fileSystem, IPathResolver pathResolver)
            : base(fileSystem, pathResolver)
        {
        }

        public void Save(PackageManifest manifest)
        {
            var existing = Load();

            var updatedList = existing.Where(c => c.Id != manifest.Id).ToList();

            updatedList.Add(new InstalledPackage
            {
                Id = manifest.Id,
                Tag = manifest.Tag,
                Version = manifest.Version,
                InstallDate = DateTime.Now
            });

            Save(updatedList.OrderBy(c => c.Id).ToList());
        }

        protected override string Name => "packages";

        public void Handle(InstallationSuccessfulEvent message)
        {
            Save(message.Manifest);
        }
    }
}