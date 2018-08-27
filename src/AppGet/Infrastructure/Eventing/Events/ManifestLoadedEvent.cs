using AppGet.Manifest;

namespace AppGet.Infrastructure.Eventing.Events
{
    public class ManifestLoadedEvent : IEvent
    {
        public object Sender { get; }
        public PackageManifest Manifest { get; }

        public ManifestLoadedEvent(object sender, PackageManifest manifest)
        {
            Sender = sender;
            Manifest = manifest;
        }
    }
}