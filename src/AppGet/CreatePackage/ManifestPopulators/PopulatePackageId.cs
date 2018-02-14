using System.Diagnostics;
using System.Text.RegularExpressions;
using AppGet.CommandLine.Prompts;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulatePackageId : IPopulateManifest
    {
        private readonly IPrompt<string> _prompt;
        private readonly Regex _idRegex = new Regex("\\W+");

        public PopulatePackageId(IPrompt<string> prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo)
        {
            var defaultValue = _idRegex.Replace(manifest.Name, "-").ToLowerInvariant().Trim('-');
            manifest.Id = _prompt.Request("Package ID", defaultValue.ToLowerInvariant());
        }
    }
}