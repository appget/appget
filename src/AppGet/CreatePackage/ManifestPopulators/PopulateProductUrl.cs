using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateProductUrl : IPopulateManifest
    {
        private readonly IUrlPrompt _prompt;

        private static readonly Regex HostNameRegex = new Regex(@"(download|update|mirror|^dl)\w*\.", RegexOptions.IgnoreCase);
        private static readonly Regex DedicatedFileHost = new Regex(@"(\.s3\.amazonaws\.)|(fosshub\.com)", RegexOptions.IgnoreCase);

        public PopulateProductUrl(IUrlPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo)
        {
            string defaultValue;
            var url = manifest.Installers.First().Location;


            var githubUrl = new GithubUrl(url);
            if (githubUrl.IsValid)
            {
                defaultValue = githubUrl.RepositoryUrl;
            }
            else
            {
                var uri = new Uri(url);
                if (DedicatedFileHost.IsMatch(uri.Host))
                {
                    defaultValue = "";
                }
                else
                {
                    defaultValue = $"{uri.Scheme}://{HostNameRegex.Replace(uri.Host, "")}";
                }
            }

            defaultValue = defaultValue.Trim().ToLowerInvariant();

            manifest.ProductUrl = _prompt.Request("Product Homepage", defaultValue).ToLowerInvariant();
        }
    }
}

