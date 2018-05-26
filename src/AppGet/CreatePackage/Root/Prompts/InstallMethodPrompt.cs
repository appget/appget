using AppGet.CommandLine.Prompts;
using AppGet.Manifest;
using AppGet.Manifest.Builder;
using AppGet.Manifests;

namespace AppGet.CreatePackage.Root.Prompts
{
    public class InstallMethodPrompt : IManifestPrompt
    {
        public bool ShouldPrompt(PackageManifestBuilder manifestBuilder)
        {
            return !manifestBuilder.InstallMethod.HasConfidence(Confidence.Plausible);
        }

        public void Invoke(PackageManifestBuilder manifest)
        {
            var methodPrompt = new EnumPrompt<InstallMethodTypes>();
            manifest.InstallMethod.Add(methodPrompt.Request("Installer", InstallMethodTypes.Custom), Confidence.Plausible, this);
        }
    }
}