using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
using AppGet.Github.Repository;
using AppGet.Manifests;
using NLog;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateProductUrl : IPopulateManifest
    {
        private readonly IUrlPrompt _prompt;
        private readonly Logger _logger;

        private static readonly Regex HostNameRegex = new Regex(@"(download|update|mirror|^repo|^dl)\w*", RegexOptions.IgnoreCase);
        private static readonly Regex DedicatedFileHost = new Regex(@"(\.s3\.amazonaws\.)|(fosshub\.com)|(akamaihd\.net)", RegexOptions.IgnoreCase);

        public PopulateProductUrl(IUrlPrompt prompt, IGitHubRepositoryClient repositoryClient, Logger logger)
        {
            _prompt = prompt;
            _logger = logger;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            if (manifest.ProductUrl != null) return;

            string defaultValue;
            var url = manifest.Installers.First().Location;


            var uri = new Uri(url);
            if (DedicatedFileHost.IsMatch(uri.Host))
            {
                defaultValue = "";
            }
            else
            {
                var domainParts = uri.Host.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                var subDomainsCount = Math.Max(0, domainParts.Length - 2);
                var subs = domainParts.Take(subDomainsCount).ToList();
                var host = domainParts.Skip(subDomainsCount).ToList();

                for (var i = subs.Count; i > 0; i--)
                {
                    if (!HostNameRegex.IsMatch(subs[i - 1]))
                    {
                        host.Insert(0, subs[i - 1]);
                    }
                    else
                    {
                        break;
                    }
                }

                defaultValue = $"{uri.Scheme}://{string.Join(".", host)}";
            }

            defaultValue = defaultValue.Trim().ToLowerInvariant();

            manifest.ProductUrl = _prompt.Request("Product Homepage", defaultValue, interactive)?.ToLowerInvariant();
        }
    }
}

