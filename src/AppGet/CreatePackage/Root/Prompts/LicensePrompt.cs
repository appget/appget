using AppGet.CommandLine.Prompts;

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
            return manifestBuilder.Version.HasConfidence(Confidence.Plausible);
        }

        public void Invoke(PackageManifestBuilder manifest)
        {
            manifest.Licence.Add(_prompt.Request("License", manifest.Licence.Value), Confidence.Authoritative, this);
        }
    }
}

