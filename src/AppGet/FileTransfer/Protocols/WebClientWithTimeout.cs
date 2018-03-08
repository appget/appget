using System;
using System.Net;

namespace AppGet.FileTransfer.Protocols
{
    public class WebClientWithTimeout : WebClient
    {
        private readonly TimeSpan _timeout;

        public WebClientWithTimeout(TimeSpan timeout)
        {
            _timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            var webRequest = base.GetWebRequest(uri);
            if (webRequest != null)
            {
                webRequest.Timeout = (int)_timeout.TotalMilliseconds;
            }
            return webRequest;
        }
    }
}