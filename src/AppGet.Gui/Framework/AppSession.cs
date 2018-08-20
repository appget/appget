using AppGet.Infrastructure.Events;
using AppGet.Manifest;

namespace AppGet.Gui.Framework
{
    public class AppSession : IStartupHandler
    {
        private readonly ITinyMessengerHub _hub;

        public AppSession(ITinyMessengerHub hub)
        {
            _hub = hub;
        }

        public void OnApplicationStartup()
        {
            _hub.Subscribe<ManifestLoadedEvent>(e =>
            {
                CurrentManifest = e.Manifest;
            });
        }

        public static PackageManifest CurrentManifest { get; private set; }
    }
}
