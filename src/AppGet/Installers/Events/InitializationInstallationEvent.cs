using AppGet.Infrastructure.Events;
using AppGet.Manifest;

namespace AppGet.Installers.Events
{
    public class InitializationInstallationEvent : StatusUpdateEvent
    {
        public InitializationInstallationEvent(object sender, PackageManifest manifest)
            : base(sender)
        {
            this.Message = $"Initializing installation of {manifest.Name}";
        }
    }

    public class ExecutingInstallerEvent : StatusUpdateEvent
    {
        public ExecutingInstallerEvent(object sender, PackageManifest manifest)
            : base(sender)
        {
            Message = $"Installing {manifest.Name} {manifest.Version}".Trim();
        }
    }

    public class InstallationSuccessfulEvent : StatusUpdateEvent
    {
        public PackageManifest Manifest { get; }

        public InstallationSuccessfulEvent(object sender, PackageManifest manifest)
            : base(sender)
        {
            Manifest = manifest;
            Message = $"{manifest.Name} {manifest.Version} Installed.".Trim();
        }
    }
}