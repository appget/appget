using System.Net.Http;
using AppGet.Exceptions;

namespace AppGet.Http
{
    public class HttpException : AppGetException
    {
        public HttpResponseMessage Response { get; }

        public HttpException(HttpResponseMessage response)
            : base($"Request to {response.RequestMessage.RequestUri} failed. {(int)response.StatusCode}:{response.StatusCode}")
        {
            Response = response;
        }
    }
}