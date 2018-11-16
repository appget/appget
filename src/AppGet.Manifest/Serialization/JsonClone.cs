
namespace AppGet.Manifest.Serialization
{
    public static class JsonClone
    {
        public static T Clone<T>(this T source) where T : new()
        {
            var json = Json.Serialize(source);
            return Json.Deserialize<T>(json);
        }
    }
}