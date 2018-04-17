using System.Collections.Generic;
using System.Linq;
using AppGet.CreatePackage.ManifestBuilder;
using AppGet.CreatePackage.Root;

namespace AppGet.CreatePackage
{
    public interface IComposeManifest
    {
        void Compose(PackageManifestBuilder manifestBuilder, bool interactive);
    }

    public class ManifestComposer : IComposeManifest
    {
        private readonly IEnumerable<IManifestPrompt> _prompts;

        public ManifestComposer(IEnumerable<IManifestPrompt> prompts)
        {
            _prompts = prompts;
        }

        public void Compose(PackageManifestBuilder manifestBuilder, bool interactive)
        {
            if (!interactive) return;

            foreach (var prompt in _prompts.Where(c => c.ShouldPrompt(manifestBuilder)))
            {
                prompt.Invoke(manifestBuilder);
            }
        }
    }
}