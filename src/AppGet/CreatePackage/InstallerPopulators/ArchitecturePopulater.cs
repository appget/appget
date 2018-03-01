using System;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
using AppGet.Manifests;

namespace AppGet.CreatePackage.InstallerPopulators
{
    public class ArchitecturePopulater : IPopulateInstaller
    {
        public void Populate(Installer installer, bool interactive)
        {
            var defaultArch = ArchitectureParser.Parse(new Uri(installer.Location));;

            var prompt = new EnumPrompt<ArchitectureTypes>();
            installer.Architecture = prompt.Request("Architecture", defaultArch, interactive);
        }
    }
}