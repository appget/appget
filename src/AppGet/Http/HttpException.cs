using System;
using AppGet.Exceptions;

namespace AppGet.Http
{
    public class HttpException : AppGetException
    {
        public HttpRequest Request { get; private set; }
        public HttpResponse Response { get; private set; }

        public HttpException(HttpRequest request, HttpResponse response)
            : base("HTTP request failed: " + response)
        {
            Request = request;
            Response = response;
        }

        public HttpException(HttpResponse response)
            : this(response.Request, response)
        {

        }
    }
}