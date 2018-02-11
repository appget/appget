using System;
using System.Diagnostics;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
using AppGet.Manifests;

namespace AppGet.CreatePackage.InstallerPopulators
{
    public class ArchitecturePopulater : IPopulateInstaller
    {
        public void Populate(Installer installer, PackageManifest manifest, FileVersionInfo fileVersionInfo)
        {
            var prompt = new EnumPrompt<ArchitectureTypes>();

            var defaultArch = ArchitectureParser.Parse(new Uri(installer.Location));

            installer.Architecture = prompt.Request("Target architecture", defaultArch);
        }
    }

    public class MinWindowsVersionPopulater : IPopulateInstaller
    {
        public void Populate(Installer installer, PackageManifest manifest, FileVersionInfo fileVersionInfo)
        {
            var prompt = new EnumPrompt<ArchitectureTypes>();

            var defaultArch = ArchitectureParser.Parse(new Uri(installer.Location));

            installer.Architecture = prompt.Request("Target architecture", defaultArch);
        }
    }
}