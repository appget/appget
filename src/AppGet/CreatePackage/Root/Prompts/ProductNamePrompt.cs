using AppGet.CommandLine.Prompts;
using AppGet.Manifest.Builder;

namespace AppGet.CreatePackage.Root.Prompts
{
    public class ProductNamePrompt : IManifestPrompt
    {
        private readonly TextPrompt _prompt;

        public ProductNamePrompt(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public bool ShouldPrompt(PackageManifestBuilder manifestBuilder)
        {
            return !manifestBuilder.Name.HasConfidence(Confidence.Authoritative);
        }

        public void Invoke(PackageManifestBuilder manifest)
        {
            var name = _prompt.Request("Product Name", manifest.Name.Value);

            manifest.Name.Add(name, Confidence.Authoritative, this);
        }
    }
}