using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AppGet.Serialization
{
    public static class Json
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static string Serialize(object obj, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting, Settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}