using AppGet.CommandLine.Prompts;

namespace AppGet.CreatePackage.Root.Prompts
{
    public class VersionTagPrompt : IManifestPrompt
    {
        private readonly TextPrompt _prompt;
        private const string LATEST = "latest";

        public VersionTagPrompt(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public bool ShouldPrompt(PackageManifestBuilder manifestBuilder)
        {
            return manifestBuilder.Version.HasConfidence(Confidence.Authoritative);
        }

        public void Invoke(PackageManifestBuilder manifest)
        {
            var tag = _prompt.Request("Version Tag", LATEST).ToLowerInvariant();

            if (tag == LATEST)
            {
                tag = null;
            }

            manifest.VersionTag = tag;
        }
    }
}