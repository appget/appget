using AppGet.Infrastructure.Eventing.Events;

namespace AppGet.FileTransfer
{
    public class FileTransferCompletedEvent : StatusUpdateEvent
    {
        public string Source { get; }
        public string Destination { get; }

        public FileTransferCompletedEvent(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}