using System.Net.Http;
using System.Threading.Tasks;
using AppGet.Manifest.Serialization;

namespace AppGet.Http
{
    public static class HttpExtension
    {
        public static async Task<string> ReadAsString(this HttpContent content)
        {
            return await content.ReadAsStringAsync();
        }

        public static async Task<T> Deserialize<T>(this HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return Json.Deserialize<T>(json);
        }
    }
}