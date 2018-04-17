using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.ManifestBuilder;

namespace AppGet.CreatePackage.Root.Prompts
{
    public class VersionPrompt : IManifestPrompt
    {
        private readonly TextPrompt _prompt;

        public VersionPrompt(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public bool ShouldPrompt(PackageManifestBuilder manifestBuilder)
        {
            return !manifestBuilder.Version.HasConfidence(Confidence.Authoritative);
        }

        public void Invoke(PackageManifestBuilder manifest)
        {
            manifest.Version.Add(_prompt.Request("Application Version", manifest.Version.Value), Confidence.Plausible, this);
        }
    }
}