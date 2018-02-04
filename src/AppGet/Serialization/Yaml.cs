using System;
using System.Globalization;
using System.IO;
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
                RegisterTypeConverter(serializer.RegisterTypeConverter);

                serializer.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }

        public static T Deserialize<T>(string text)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .IgnoreUnmatchedProperties()
                .WithTypeConverter(new VersionConverter())
                .WithTypeConverter(new WindowsVersionConverter())
                .Build();

            return deserializer.Deserialize<T>(text);
        }

        private static void RegisterTypeConverter(Action<IYamlTypeConverter> registerConverter)
        {
            registerConverter(new VersionConverter());
            registerConverter(new WindowsVersionConverter());
        }
    }
}