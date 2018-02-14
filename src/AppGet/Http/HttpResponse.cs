using System.Net;
using AppGet.Serialization;

namespace AppGet.Http
{
    public class HttpResponse
    {
        public HttpResponse(HttpRequest request, HttpHeader headers, string content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Request = request;
            Headers = headers;
            Content = content;
            StatusCode = statusCode;
        }

        public HttpRequest Request { get; }
        public HttpHeader Headers { get; }
        public HttpStatusCode StatusCode { get; }


        public string Content { get; set; }


        public bool HasHttpError => (int)StatusCode >= 400;

        public override string ToString()
        {
            var result = $"Res: [{Request.Method}] {Request.Url} : {(int) StatusCode}.{StatusCode}";
            return result;
        }
    }


    public class HttpResponse<T> : HttpResponse where T : new()
    {
        public HttpResponse(HttpResponse response)
            : base(response.Request, response.Headers, response.Content, response.StatusCode)
        {
            Resource = Json.Deserialize<T>(response.Content);
        }

        public T Resource { get; private set; }
    }
}