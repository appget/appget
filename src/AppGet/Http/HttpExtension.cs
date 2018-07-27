using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AppGet.Manifest.Serialization;

namespace AppGet.Http
{
    public static class HttpExtension
    {
        public static string ReadAsString(this HttpContent content)
        {
            try
            {
                return content.ReadAsStringAsync().Result;
            }
            catch (AggregateException ex)
            {
                throw ex.Flatten().InnerExceptions.First();
            }
        }

        public static T Deserialize<T>(this HttpResponseMessage response)
        {
            return Json.Deserialize<T>(response.Content.ReadAsString());
        }
    }
}