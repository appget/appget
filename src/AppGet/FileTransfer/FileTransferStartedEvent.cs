using AppGet.Infrastructure.Eventing.Events;

namespace AppGet.FileTransfer
{
    public class FileTransferStartedEvent : StatusUpdateEvent
    {
        public string Source { get; }
        public string Destination { get; }

        public FileTransferStartedEvent(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}