using System.Net.Http;
using AppGet.Exceptions;

namespace AppGet.Http
{
    public class HttpTimeoutException : AppGetException
    {
        public HttpRequestMessage Request { get; }

        public HttpTimeoutException(HttpRequestMessage req)
            : base($"Request to {req.RequestUri} has timed out.")
        {
            Request = req;
        }
    }
}