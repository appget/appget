using System;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.ManifestBuilder;
using AppGet.Manifests;

namespace AppGet.CreatePackage.Installer.Prompts
{
    public class MinWindowsVersionPrompt : IInstallerPrompt
    {
        private readonly WindowsVersionPrompt _prompt;

        public MinWindowsVersionPrompt(WindowsVersionPrompt prompt)
        {
            _prompt = prompt;
        }

        public bool ShouldPrompt(InstallerBuilder installerBuilder)
        {
            return !installerBuilder.MinWindowsVersion.HasConfidence(Confidence.Authoritative);
        }

        public void Invoke(InstallerBuilder installerBuilder)
        {
            var minWindowsVersion = _prompt.Request("Minimum Windows Version", WindowsVersion.KnownVersions.First());

            if (minWindowsVersion != new Version(0, 0))
            {
                installerBuilder.MinWindowsVersion.Add(minWindowsVersion, Confidence.Authoritative, this);
            }
        }
    }
}