using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.Compression;
using AppGet.Installers;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateInstallMethod : IPopulateManifest
    {
        private readonly TextPrompt _prompt;
        private readonly IEnumerable<IDetectInstallMethod> _installMethodDetectors;

        public PopulateInstallMethod(TextPrompt prompt, ICompressionService compressionService, IEnumerable<IDetectInstallMethod> installMethodDetectors)
        {
            _prompt = prompt;
            _installMethodDetectors = installMethodDetectors;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo)
        {
            var installer = manifest.Installers.First();
//            var archiveContent = _
//
//            foreach (var detector in _installMethodDetectors)
//            {
//                    detector.GetConfidence(installer.FilePath,)
//            }
        }
    }
}

