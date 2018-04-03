using System.Net.Http;
using AppGet.Exceptions;

namespace AppGet.Http
{
    public class HttpException : AppGetException
    {
        public HttpResponseMessage Response { get; }

        public HttpException(HttpResponseMessage response)
            : base($"{response.RequestMessage.RequestUri} {response.StatusCode}")
        {
            Response = response;
        }
    }
}