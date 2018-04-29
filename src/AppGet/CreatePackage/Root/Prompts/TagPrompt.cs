using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.ManifestBuilder;

namespace AppGet.CreatePackage.Root.Prompts
{
    public class TagPrompt : IManifestPrompt
    {
        private readonly TextPrompt _prompt;
        private const string LATEST = "latest";

        public TagPrompt(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public bool ShouldPrompt(PackageManifestBuilder manifestBuilder)
        {
            return true;
        }

        public void Invoke(PackageManifestBuilder manifest)
        {
            var tag = _prompt.Request("Tag", LATEST)?.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(tag) || tag == LATEST)
            {
                tag = null;
            }

            manifest.Tag = tag;
        }
    }
}