using AppGet.CommandLine.Prompts;

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
            return manifestBuilder.Version.HasConfidence(Confidence.Authoritive);
        }

        public void Invoke(PackageManifestBuilder manifest)
        {
            manifest.Version.Add(_prompt.Request("Application Version", manifest.Version.Top), Confidence.Reasonable, this);
        }
    }
}

