using Newtonsoft.Json;

namespace AppGet.Manifest.Serialization
{
    public static class JsonClone
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings();

        static JsonClone()
        {
            Settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            Settings.NullValueHandling = NullValueHandling.Ignore;
            Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }


        public static T Clone<T>(this T source) where T : new()
        {
            var json = JsonConvert.SerializeObject(source, Settings);
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
    }
}