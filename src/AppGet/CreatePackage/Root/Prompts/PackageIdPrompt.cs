using AppGet.CommandLine.Prompts;
using AppGet.Manifest;
using AppGet.Manifest.Builder;

namespace AppGet.CreatePackage.Root.Prompts
{
    public class PackageIdPrompt : IManifestPrompt
    {
        private readonly TextPrompt _prompt;

        public PackageIdPrompt(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public bool ShouldPrompt(PackageManifestBuilder manifestBuilder)
        {
            return !manifestBuilder.Id.HasConfidence(Confidence.Authoritative);
        }

        public void Invoke(PackageManifestBuilder manifestBuilder)
        {
            var suggestion = manifestBuilder.Name.Value.ToPackageId();
            var id = _prompt.Request("Package ID", suggestion);
            manifestBuilder.Id.Add(id.ToPackageId(), Confidence.Authoritative, this);
        }
    }
}