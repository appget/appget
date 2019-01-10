using System.Threading.Tasks;
using AppGet.Commands;
using AppGet.Infrastructure.Eventing;
using AppGet.Installers.Events;
using AppGet.Manifest;

namespace AppGet.Gui.Framework
{
    public class AppSession : IHandle<InstallationInitializedEvent>
    {
        public static PackageManifest CurrentManifest { get; private set; }
        public static AppGetOption Options { get; set; }

        public Task Handle(InstallationInitializedEvent @event)
        {
            CurrentManifest = @event.Manifest;
            return Task.FromResult(0);
        }
    }
}
