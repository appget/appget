using AppGet.CommandLine.Prompts;
using AppGet.Manifests;

namespace AppGet.CreatePackage.Installer.Prompts
{
    public class ArchitecturePrompt : IInstallerPrompt
    {
        public bool ShouldPrompt(InstallerBuilder installerBuilder)
        {
            return !installerBuilder.Architecture.HasConfidence(Confidence.Authoritative);
        }

        public void Invoke(InstallerBuilder installer)
        {
            var prompt = new EnumPrompt<ArchitectureTypes>();
            var value = prompt.Request("Architecture", installer.Architecture.Value);
            installer.Architecture.Add(value, Confidence.Authoritative, this);
        }
    }
}