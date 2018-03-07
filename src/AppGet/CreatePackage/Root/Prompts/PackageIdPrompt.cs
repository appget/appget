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
            return !manifestBuilder.Id.HasConfidence(Confidence.Authoritative);
        }

        public void Invoke(PackageManifestBuilder manifestBuilder)
        {
            manifestBuilder.Id.Add(_prompt.Request("Package ID", manifestBuilder.Id.Value), Confidence.Authoritative, this);
        }
    }
}