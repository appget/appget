using System;

namespace AppGet.Download
{
    public class DownloadClientNotFoundException : Exception
    {
        public DownloadClientNotFoundException(string message, params object[] args)
            : base(String.Format(message, args))
        {
        }
    }
}
