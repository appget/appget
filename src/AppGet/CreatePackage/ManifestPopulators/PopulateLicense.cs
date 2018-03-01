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

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            if (manifest.Licence != null) return;

            manifest.Licence = _prompt.Request("License", null, interactive);
        }
    }
}

