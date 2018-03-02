using System.Collections.Generic;
using System.Linq;
using AppGet.CreatePackage.Root;

namespace AppGet.CreatePackage
{
    public interface IComposeManifest
    {
        void Compose(PackageManifestBuilder manifestBuilder, bool interactive);
    }

    public class ManifestComposer : IComposeManifest
    {
        private readonly IEnumerable<IExtractToManifestRoot> _populaters;
        private readonly IEnumerable<IManifestPrompt> _prompts;

        public ManifestComposer(IEnumerable<IExtractToManifestRoot> populaters, IEnumerable<IManifestPrompt> prompts)
        {
            _populaters = populaters;
            _prompts = prompts;
        }


        public void Compose(PackageManifestBuilder manifestBuilder, bool interactive)
        {
            foreach (var populater in _populaters)
            {
                populater.Invoke(manifestBuilder);
            }

            if (!interactive) return;

            foreach (var prompt in _prompts.Where(c => c.ShouldPrompt(manifestBuilder)))
            {
                prompt.Invoke(manifestBuilder);
            }
        }
    }
}