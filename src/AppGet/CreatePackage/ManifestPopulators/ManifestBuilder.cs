using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public interface IManifestBuilder
    {
        void Populate(PackageManifestBuilder manifestBuilder, bool interactive);
    }

    public class ManifestBuilder : IManifestBuilder
    {
        private readonly IEnumerable<IPopulateManifest> _populaters;

        public ManifestBuilder(IEnumerable<IPopulateManifest> populaters)
        {
            _populaters = populaters;
        }


        public void Populate(PackageManifestBuilder manifestBuilder, bool interactive)
        {
            var installer = manifestBuilder.Installers.First();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(installer.FilePath);

            foreach (var populater in _populaters)
            {
                populater.Populate(manifestBuilder, fileVersionInfo, interactive);
            }
        }
    }
}