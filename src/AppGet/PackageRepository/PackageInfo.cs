using System.ComponentModel;
using Newtonsoft.Json;

namespace AppGet.PackageRepository
{
    public class PackageInfo
    {
        public string Id { get; set; }

        [DefaultValue("latest")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Tag { get; set; }
        public string Version { get; set; }
        public string ManifestPath { get; set; }

        public override string ToString()
        {
            var text = $"{Id}";
            if (Tag != null)
            {
                text += $":{Tag}";
            }

            return text;
        }
    }
}