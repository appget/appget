using System.Diagnostics;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateProductName : IPopulateManifest
    {
        private readonly TextPrompt _prompt;


        public PopulateProductName(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            string defaultValue = null;

            if (fileVersionInfo != null)
            {
                defaultValue = new[] { fileVersionInfo.ProductName, fileVersionInfo.InternalName, fileVersionInfo.CompanyName }
                    .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))?.Trim();
            }


            if (string.IsNullOrWhiteSpace(defaultValue))
            {
                defaultValue = manifest.Name;
            }

            manifest.Name = _prompt.Request("Product Name", defaultValue, interactive);
        }
    }
}