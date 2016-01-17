using System;
using System.Collections.Generic;
using System.Net;

namespace AppGet.Http
{
    public class HttpRequest
    {
        private readonly Dictionary<string, string> _segments;

        public HttpRequest(string url, HttpAccept httpAccept = null)
        {
            UriBuilder = new UriBuilder(url);
            Headers = new HttpHeader();
            _segments = new Dictionary<string, string>();
            AllowAutoRedirect = true;

            if (httpAccept != null)
            {
                Headers.Accept = httpAccept.Value;
            }
        }

        public UriBuilder UriBuilder { get; }

        public Uri Url
        {
            get
            {
                var uri = UriBuilder.Uri.ToString();

                foreach (var segment in _segments)
                {
                    uri = uri.Replace(segment.Key, segment.Value);
                }

                return new Uri(uri);
            }
        }

        public HttpMethod Method { get; set; }
        public HttpHeader Headers { get; set; }
        public string Body { get; set; }
        public NetworkCredential NetworkCredential { get; set; }
        public bool SuppressHttpError { get; set; }
        public bool AllowAutoRedirect { get; set; }

        public override string ToString()
        {
            if (Body == null)
            {
                return $"Req: [{Method}] {Url}";
            }

            return $"Req: [{Method}] {Url} {System.Environment.NewLine} {Body}";
        }

        public void AddSegment(string segment, string value)
        {
            var key = "{" + segment + "}";

            if (!UriBuilder.Uri.ToString().Contains(key))
            {
                throw new InvalidOperationException("Segment " + key + " is not defined in Uri");
            }

            _segments.Add(key, value);
        }

        public void DisableCache()
        {
            Headers.CacheControl = "no-cache";

            if (!string.IsNullOrEmpty(UriBuilder.Query))
            {
                UriBuilder.Query += "&";
            }

            UriBuilder.Query += "no_cache=" + DateTime.Now.Ticks;
        }
    }
}