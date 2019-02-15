using AppGet.CommandLine.Prompts;
using AppGet.Manifest;
using AppGet.Manifest.Builder;

namespace AppGet.CreatePackage.Root.Prompts
{
    public class TagPrompt : IManifestPrompt
    {
        private readonly TextPrompt _prompt;

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
            var tag = _prompt.Request("Tag", PackageManifest.LATEST_TAG)?.ToLowerInvariant();

            if (TagHelper.IsLatest(tag))
            {
                tag = null;
            }

            manifest.Tag = tag;
        }
    }
}