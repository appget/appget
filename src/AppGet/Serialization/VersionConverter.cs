using System;
using AppGet.Manifests;
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
            return type == typeof(Version);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var value = ((Scalar)parser.Current).Value;
            var version = new Version(value);

            parser.MoveNext();
            return version;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            if (value != null)
            {
                emitter.Emit(new Scalar(((Version)value).ToString(2)));
            }
        }
    }


    public class WindowsVersionConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(WindowsVersion);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var value = ((Scalar)parser.Current).Value;
            var version = WindowsVersion.Parse(value);

            parser.MoveNext();
            return version;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            if (value != null)
            {
                emitter.Emit(new Scalar(value.ToString()));
            }
        }
    }
}
