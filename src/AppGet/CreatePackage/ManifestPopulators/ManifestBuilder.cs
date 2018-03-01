using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public interface IManifestBuilder
    {
        void Populate(PackageManifest manifest, bool interactive);
    }

    public class ManifestBuilder : IManifestBuilder
    {
        private readonly IEnumerable<IPopulateManifest> _populaters;

        public ManifestBuilder(IEnumerable<IPopulateManifest> populaters)
        {
            _populaters = populaters;
        }


        public void Populate(PackageManifest manifest, bool interactive)
        {
            var installer = manifest.Installers.First();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(installer.FilePath);

            foreach (var populater in _populaters)
            {
                populater.Populate(manifest, fileVersionInfo, interactive);
            }
        }
    }
}