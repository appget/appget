using System;
using System.Collections.Generic;
using System.Net.Http;
using AppGet.Serialization;

namespace AppGet.Http
{
    public static class HttpExtension
    {
        public static T AsResource<T>(this HttpResponseMessage response)
        {
            return Json.Deserialize<T>(response.Content.ReadAsStreamAsync().Result);
        }

        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            if (!String.IsNullOrWhiteSpace(uri.Query))
            {
                throw new NotImplementedException("Can't add query string to URI that already has one.");
            }

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(name, value.Trim())
            });

            var query = content.ReadAsStringAsync().Result;

            var builder = new UriBuilder(uri) { Query = query };

            return builder.Uri;
        }

        public static void SetQueryParam(this UriBuilder uriBuilder, string key, object value)
        {
            var query = uriBuilder.Query;

            if (!String.IsNullOrEmpty(query))
            {
                query += "&";
            }

            uriBuilder.Query = query.Trim('?') + (key + "=" + value);
        }
    }
}
