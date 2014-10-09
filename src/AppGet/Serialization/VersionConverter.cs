using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Version = System.Version;

namespace AppGet.Serialization
{
    public class VersionConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof (Version);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var value = ((Scalar) parser.Current).Value;
            var version = new Version(value);

            parser.MoveNext();
            return version;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            emitter.Emit(new Scalar(((Version)value).ToString(2)));
        }
    }
}
