using AppGet.CommandLine.Prompts;
using AppGet.Manifest.Builder;

namespace AppGet.CreatePackage.Root.Prompts
{
    public class LicensePrompt : IManifestPrompt
    {
        private readonly TextPrompt _prompt;

        public LicensePrompt(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public bool ShouldPrompt(PackageManifestBuilder manifestBuilder)
        {
            return !manifestBuilder.License.HasConfidence(Confidence.Plausible);
        }

        public void Invoke(PackageManifestBuilder manifest)
        {
            manifest.License.Add(_prompt.Request("License", manifest.License.Value), Confidence.Authoritative, this);
        }
    }
}