using System;
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

        public void Populate(Installer installer, bool interactive)
        {
            var minWindowsVersion = _prompt.Request("Minimum Windows Version", WindowsVersion.KnownVersions.First(), interactive);

            if (minWindowsVersion != new Version(0, 0))
            {
                installer.MinWindowsVersion = minWindowsVersion;
            }

        }
    }
}