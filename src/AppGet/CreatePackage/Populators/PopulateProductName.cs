using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Utils;
using AppGet.Manifests;

namespace AppGet.CreatePackage.Populators
{
    public class PopulateProductName : IPopulateManifest
    {
        private readonly IPrompt _prompt;
        readonly TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;


        public PopulateProductName(IPrompt prompt)
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
                    defaultValue = githubUrl.OrganizationName;
                }
                else
                {
                    var uri = new Uri(manifest.ProductUrl);
                    defaultValue = uri.Host.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).First(c => c.ToLower() != "www");
                }

                defaultValue = _textInfo.ToTitleCase(defaultValue);
            }

            manifest.Name = _prompt.Request("Product Name", defaultValue);
        }
    }
}