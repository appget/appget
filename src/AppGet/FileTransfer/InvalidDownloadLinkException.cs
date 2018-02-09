using AppGet.Exceptions;

namespace AppGet.FileTransfer
{
    public class InvalidDownloadLinkException : AppGetException
    {
        public InvalidDownloadLinkException(string url)
            : base($"Invalid download URL {url}.")
        {
        }
    }
}
