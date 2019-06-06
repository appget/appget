using System.Net.Http;
using System.Text;
using AppGet.Manifest.Serialization;

namespace AppGet.Http
{
    public class JsonContent : StringContent
    {
        public JsonContent(object obj)
            : base(Json.Serialize(obj), Encoding.UTF8, "Application/Json")
        {
        }
    }
}