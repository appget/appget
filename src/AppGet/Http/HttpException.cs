using System;
using System.Net.Http;

namespace AppGet.Http
{
    public class HttpException : Exception
    {
        public HttpResponseMessage Response { get; }

        public HttpException(HttpResponseMessage response)
        {
            Response = response;
        }
    }
}