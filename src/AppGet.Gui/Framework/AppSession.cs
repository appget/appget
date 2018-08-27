using AppGet.Infrastructure.Eventing;
using AppGet.Infrastructure.Eventing.Events;
using AppGet.Manifest;

namespace AppGet.Gui.Framework
{
    public class AppSession : IHandle<ManifestLoadedEvent>
    {
        public static PackageManifest CurrentManifest { get; private set; }
        public void Handle(ManifestLoadedEvent @event)
        {
            CurrentManifest = @event.Manifest;
        }
    }
}
