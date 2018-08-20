using AppGet.Manifest;

namespace AppGet.Infrastructure.Events
{
    public class ManifestLoadedEvent : ITinyMessage
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