using System.Globalization;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AppGet.Serialization
{
    public static class Yaml
    {
        public static string Serialize(object obj)
        {
            using (var textWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var serializer = new Serializer(SerializationOptions.EmitDefaults | SerializationOptions.DisableAliases, new CamelCaseNamingConvention());
                serializer.RegisterTypeConverter(new VersionConverter());

                serializer.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }

        public static T Deserialize<T>(string text)
        {
            var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
            deserializer.RegisterTypeConverter(new VersionConverter());

            var reader = new EventReader(new Parser(new StringReader(text)));
            return deserializer.Deserialize<T>(reader);
        }
    }
}