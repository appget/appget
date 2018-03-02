using System.Diagnostics;
using AppGet.CommandLine.Prompts;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateVersionTag : IPopulateManifest
    {
        private readonly TextPrompt _prompt;
        private const string LATEST = "latest";

        public PopulateVersionTag(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifestBuilder manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            if (!interactive) return;

            var tag = _prompt.Request("Version Tag", LATEST).ToLowerInvariant();

            if (tag == LATEST)
            {
                tag = null;
            }

            manifest.VersionTag = tag;
        }
    }
}