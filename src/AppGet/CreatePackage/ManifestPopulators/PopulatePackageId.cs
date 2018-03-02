using System.Diagnostics;
using System.Text.RegularExpressions;
using AppGet.CommandLine.Prompts;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulatePackageId : IPopulateManifest
    {
        private readonly TextPrompt _prompt;
        private readonly Regex _idRegex = new Regex("\\W+");

        public PopulatePackageId(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifestBuilder manifestBuilder, FileVersionInfo fileVersionInfo, bool interactive)
        {
            var id = _idRegex.Replace(manifestBuilder.Name.Top, "-").ToLowerInvariant().Trim('-');
            manifestBuilder.Id.Add(id.Replace("+", "plus"), Confidence.Reasonable, this);

            if (!interactive || manifestBuilder.Id.HasConfidence(Confidence.VeryHigh)) return;
            manifestBuilder.Id.Add(_prompt.Request("Package ID", manifestBuilder.Id.Top), Confidence.VeryHigh, this);
        }
    }
}