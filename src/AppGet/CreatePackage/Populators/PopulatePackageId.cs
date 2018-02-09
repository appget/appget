using System.Text.RegularExpressions;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Utils;
using AppGet.Manifests;

namespace AppGet.CreatePackage.Populators
{
    public class PopulatePackageId : IPopulateManifest
    {
        private readonly IPrompt _prompt;
        private readonly Regex _idRegex = new Regex("\\W+");


        public PopulatePackageId(IPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest)
        {
            var defaultValue = _idRegex.Replace(manifest.Name, "_").ToLowerInvariant().Trim('-');
            manifest.Id = _prompt.Request("Package ID", defaultValue).ToLowerInvariant();
        }
    }
}