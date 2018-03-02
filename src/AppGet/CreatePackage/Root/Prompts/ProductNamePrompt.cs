using AppGet.CommandLine.Prompts;

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
            return manifestBuilder.Version.HasConfidence(Confidence.Authoritive);
        }

        public void Invoke(PackageManifestBuilder manifest)
        {
            manifest.Name.Add(_prompt.Request("Product Name", manifest.Name.Top), Confidence.Authoritive, this);
        }
    }
}