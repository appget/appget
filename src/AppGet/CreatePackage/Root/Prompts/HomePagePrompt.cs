using AppGet.CommandLine.Prompts;

namespace AppGet.CreatePackage.Root.Prompts
{
    public class HomePagePrompt : IManifestPrompt
    {
        private readonly IUrlPrompt _prompt;

        public HomePagePrompt(IUrlPrompt prompt)
        {
            _prompt = prompt;
        }

        public bool ShouldPrompt(PackageManifestBuilder manifestBuilder)
        {
            return !manifestBuilder.Home.HasConfidence(Confidence.Authoritative);
        }

        public void Invoke(PackageManifestBuilder manifestBuilder)
        {
            var result = _prompt.Request("Product Homepage", manifestBuilder.Home.Value);
            manifestBuilder.Home.Add(result, Confidence.Authoritative, this);
        }
    }
}