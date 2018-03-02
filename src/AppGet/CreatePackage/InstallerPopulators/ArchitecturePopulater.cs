using System;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
using AppGet.Manifests;

namespace AppGet.CreatePackage.InstallerPopulators
{
    public class ArchitecturePopulater : IPopulateInstaller
    {
        public void Populate(InstallerBuilder installer, bool interactive)
        {
            var urlArch = ArchitectureParser.Parse(new Uri(installer.Location));
            installer.Architecture.Add(urlArch, Confidence.Reasonable, this);

            if (interactive)
            {
                var prompt = new EnumPrompt<ArchitectureTypes>();
                var value = prompt.Request("Architecture", urlArch);
                installer.Architecture.Add(value, Confidence.VeryHigh, this);
            }
        }
    }
}