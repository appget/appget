using AppGet.Serialization.fastJSON;

namespace AppGet.Serialization
{
    public static class Json
    {
        public static string Serialize(object obj)
        {
            return FastJson.ToJSON(obj);
        }

        public static T Deserialize<T>(string text)
        {
            return FastJson.ToObject<T>(text, new JSONParameters { IgnoreCaseOnDeserialize = true });
        }
    }
}