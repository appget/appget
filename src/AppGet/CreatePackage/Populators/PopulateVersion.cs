using System.Diagnostics;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.Extensions;
using AppGet.Manifests;

namespace AppGet.CreatePackage.Populators
{
    public class PopulateVersion : IPopulateManifest
    {
        private readonly IPrompt _prompt;

        public PopulateVersion(IPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo)
        {
            string defaultValue = null;

            if (fileVersionInfo != null)
            {
                defaultValue = new[] { fileVersionInfo.ProductVersion, fileVersionInfo.FileVersion }
                    .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c));
            }

            manifest.Version = _prompt.Request("Application Version", defaultValue).ToLowerInvariant();
        }
    }
}

