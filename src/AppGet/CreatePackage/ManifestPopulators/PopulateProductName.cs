using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateProductName : IPopulateManifest
    {
        private readonly TextPrompt _prompt;
        readonly TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;


        public PopulateProductName(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo)
        {
            string defaultValue = null;

            if (fileVersionInfo != null)
            {
                defaultValue = new[] { fileVersionInfo.ProductName, fileVersionInfo.InternalName, fileVersionInfo.CompanyName }
                    .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c));
            }

            if (defaultValue == null)
            {
                var url = manifest.Installers.First().Location;

                var githubUrl = new GithubUrl(url);
                if (githubUrl.IsValid)
                {
                    defaultValue = _textInfo.ToTitleCase(githubUrl.OrganizationName);
                }

            }

            manifest.Name = _prompt.Request("Product Name", defaultValue);
        }
    }
}