using System;
using System.Net.Http;

namespace AppGet.Http
{
    public class AppGetApiException : Exception
    {
        public HttpResponseMessage Response { get; }
        public HttpRequestMessage Request { get; }
        public AppGetApiError ApiError { get; }

        private string body;

        public AppGetApiException(HttpResponseMessage response)
        {
            Response = response;
            Request = response.RequestMessage;

            ApiError = response.Deserialize<AppGetApiError>().Result;
        }

        public override string Message => $"{Response.StatusCode.ToString()}: {Request.RequestUri}. {ApiError}";
    }
}