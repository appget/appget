using AppGet.Exceptions;

namespace AppGet.FileTransfer
{
    public class InvalidDownloadUrlException : AppGetException
    {
        public InvalidDownloadUrlException(string url)
            : base($"Invalid download URL {url}")
        {
        }

        public InvalidDownloadUrlException(string url, string message)
            : base($"Invalid download URL {url}. {message}")
        {
        }
    }
}
