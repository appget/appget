using System.Globalization;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AppGet.Manifest.Serialization
{
    public static class Yaml
    {
        private static readonly Deserializer Deserializer = new DeserializerBuilder()
            .WithNamingConvention(new CamelCaseNamingConvention())
            .WithTypeConverter(new VersionConverter())
            .IgnoreUnmatchedProperties()
            .Build();

        public static string Serialize(object obj)
        {
            using (var textWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new SerializerBuilder()
                    .WithNamingConvention(new CamelCaseNamingConvention())
                    .WithTypeConverter(new VersionConverter())
                    .WithEmissionPhaseObjectGraphVisitor(args => new YamlDefaultGraphVisitor(args.InnerVisitor))
                    .DisableAliases()
                    .Build().Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }

        public static T Deserialize<T>(string text)
        {
            return Deserializer.Deserialize<T>(text);
        }

        public static T Deserialize<T>(Stream stream)
        {
            var tr = new StreamReader(stream);
            return Deserializer.Deserialize<T>(tr);
        }
    }
}