using AppGet.Infrastructure.Eventing.Events;
using AppGet.Manifest;

namespace AppGet.Installers.Events
{
    public class InstallationInitializedEvent : StatusUpdateEvent
    {
        public PackageManifest Manifest { get; }

        public InstallationInitializedEvent(PackageManifest manifest)
        {
            Manifest = manifest;
            Message = $"Initializing installation of {manifest.Name}";
        }
    }

    public class ExecutingInstallerEvent : StatusUpdateEvent
    {
        public ExecutingInstallerEvent(PackageManifest manifest)
        {
            Message = $"Installing {manifest.Name} {manifest.Version}".Trim();
        }
    }

    public class InstallationSuccessfulEvent : StatusUpdateEvent
    {
        public PackageManifest Manifest { get; }

        public InstallationSuccessfulEvent(PackageManifest manifest)
        {
            Manifest = manifest;
            Message = $"{manifest.Name} {manifest.Version} Installed.".Trim();
        }
    }
}