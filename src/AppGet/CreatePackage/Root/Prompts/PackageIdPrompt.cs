using AppGet.CommandLine.Prompts;

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
            return manifestBuilder.Version.HasConfidence(Confidence.Authoritive);
        }

        public void Invoke(PackageManifestBuilder manifestBuilder)
        {
            manifestBuilder.Id.Add(_prompt.Request("Package ID", manifestBuilder.Id.Top), Confidence.Authoritive, this);
        }
    }
}