using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AppGet.Manifest.Serialization
{
    public static class Yaml
    {
        private static readonly IDeserializer Deserializer = new DeserializerBuilder()
            .WithNamingConvention(new CamelCaseNamingConvention())
            .WithTypeConverter(new VersionConverter())
            .IgnoreUnmatchedProperties()
            .Build();


        private static readonly Regex YamlEmptyArray = new Regex(@"\W*\w+:\W*\[\]\W*$", RegexOptions.Compiled);

        public static string Serialize(object obj, bool ignoreEmptyArrays = false)
        {
            using (var textWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new SerializerBuilder()
                    .WithNamingConvention(new CamelCaseNamingConvention())
                    .WithTypeConverter(new VersionConverter())
                    .WithEmissionPhaseObjectGraphVisitor(args => new YamlDefaultGraphVisitor(args.InnerVisitor))
                    .DisableAliases()
                    .Build().Serialize(textWriter, obj);
                var yaml = textWriter.ToString();

                if (ignoreEmptyArrays)
                {
                    yaml = YamlEmptyArray.Replace(yaml, "");
                }

                return yaml;
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