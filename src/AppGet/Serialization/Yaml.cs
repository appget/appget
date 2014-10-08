using System.Globalization;
using System.IO;
using YamlDotNet.Serialization;

namespace AppGet.Serialization
{
    public static class Yaml
    {
        public static string Serialize(object obj)
        {
            using (var textWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var serializer = new Serializer(SerializationOptions.EmitDefaults);
               
                serializer.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }
    }
}