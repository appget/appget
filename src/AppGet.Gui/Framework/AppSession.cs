using AppGet.Infrastructure.Events;
using AppGet.Manifest;

namespace AppGet.Gui.Framework
{
    public class AppSession : IHandle<ManifestLoadedEvent>
    {
        public static PackageManifest CurrentManifest { get; private set; }

        public void Handle(ManifestLoadedEvent message)
        {
            CurrentManifest = message.Manifest;
        }
    }
}
