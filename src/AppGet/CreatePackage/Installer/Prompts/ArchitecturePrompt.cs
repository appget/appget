using AppGet.CommandLine.Prompts;
using AppGet.Manifests;

namespace AppGet.CreatePackage.Installer.Prompts
{
    public class ArchitecturePrompt : IInstallerPrompt
    {
        public bool ShouldPrompt(InstallerBuilder installerBuilder)
        {
            return installerBuilder.Architecture.HasConfidence(Confidence.Authoritive);
        }

        public void Invoke(InstallerBuilder installer)
        {
            var prompt = new EnumPrompt<ArchitectureTypes>();
            var value = prompt.Request("Architecture", installer.Architecture.Top);
            installer.Architecture.Add(value, Confidence.Authoritive, this);
        }
    }
}