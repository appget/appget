using System;

namespace AppGet.Http
{
    public static class UriExtensions
    {
        public static void SetQueryParam(this UriBuilder uriBuilder, string key, object value)
        {
            var query = uriBuilder.Query;

            if (!string.IsNullOrEmpty(query))
            {
                query += "&";
            }

            uriBuilder.Query = query.Trim('?') + (key + "=" + value);
        }


    }
}