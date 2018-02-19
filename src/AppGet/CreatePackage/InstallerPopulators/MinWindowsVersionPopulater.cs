using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.Manifests;

namespace AppGet.CreatePackage.InstallerPopulators
{
    public class MinWindowsVersionPopulater : IPopulateInstaller
    {
        private readonly WindowsVersionPrompt _prompt;

        public MinWindowsVersionPopulater(WindowsVersionPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(Installer installer)
        {
            installer.MinWindowsVersion = _prompt.Request("Minimum Windows Version", WindowsVersion.KnownVersions.First());
        }
    }
}