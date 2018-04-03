using System.Collections.Generic;
using System.Linq;

namespace AppGet.CreatePackage.Installer
{
    public interface IComposeInstaller
    {
        void Compose(InstallerBuilder installerBuilder);
    }

    public class ComposeInstaller : IComposeInstaller
    {
        private readonly IEnumerable<IInstallerPrompt> _prompts;

        public ComposeInstaller(IEnumerable<IInstallerPrompt> prompts)
        {
            _prompts = prompts;
        }

        public void Compose(InstallerBuilder installerBuilder)
        {
            foreach (var prompt in _prompts.Where(p => p.ShouldPrompt(installerBuilder)))
            {
                prompt.Invoke(installerBuilder);
            }
        }
    }
}