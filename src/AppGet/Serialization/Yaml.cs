using System.Globalization;
using System.IO;
using System.Linq;
using AppGet.Infrastructure.Composition;
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
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(new CamelCaseNamingConvention())
                    .WithTypeConverter(new VersionConverter())
                    .WithEmissionPhaseObjectGraphVisitor(args => new JsonDefaultGraphVisitor(args.InnerVisitor))
                    .DisableAliases()
                    .Build();

                serializer.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }

        public static T Deserialize<T>(string text)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .WithTypeConverter(new VersionConverter())
                .IgnoreUnmatchedProperties()
                .Build();

            return deserializer.Deserialize<T>(text);
        }
    }
}