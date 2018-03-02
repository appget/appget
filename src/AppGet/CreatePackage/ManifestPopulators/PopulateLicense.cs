using System.Diagnostics;
using AppGet.CommandLine.Prompts;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateLicense : IPopulateManifest
    {
        private readonly TextPrompt _prompt;

        public PopulateLicense(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifestBuilder manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            if (!interactive || manifest.Licence.HasConfidence(Confidence.Reasonable)) return;

            manifest.Licence.Add(_prompt.Request("License", manifest.Licence.Top), Confidence.VeryHigh, this);
        }
    }
}

