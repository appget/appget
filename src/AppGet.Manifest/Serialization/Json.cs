using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace AppGet.Manifest.Serialization
{
    public static class Json
    {
        public static readonly JsonSerializer Serializer;

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        static Json()
        {
            Settings.Converters.Add(new StringEnumConverter(false));
            Settings.Converters.Add(new Newtonsoft.Json.Converters.VersionConverter());
            Settings.NullValueHandling = NullValueHandling.Ignore;
            Settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
            Settings.NullValueHandling = NullValueHandling.Ignore;
            Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            Serializer = JsonSerializer.Create(Settings);
        }

        public static string Serialize(object obj, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting, Settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T Deserialize<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(reader))
            {
                return Serializer.Deserialize<T>(jsonTextReader);
            }
        }

        public static object Deserialize(Stream stream, Type type)
        {
            using (var reader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(reader))
            {
                return Serializer.Deserialize(jsonTextReader, type);
            }
        }
    }
}