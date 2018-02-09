using System.Diagnostics;
using AppGet.CommandLine.Prompts;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateVersionTag : IPopulateManifest
    {
        private readonly IPrompt _prompt;
        private const string LATEST = "latest";

        public PopulateVersionTag(IPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo)
        {
            var tag = _prompt.Request("VersionTag", LATEST).ToLowerInvariant();

            if (tag == LATEST)
            {
                tag = null;
            }

            manifest.VersionTag = tag;
        }
    }
}