using System.ComponentModel;
using AppGet.Manifests;
using Newtonsoft.Json;

namespace AppGet.PackageRepository
{
    public class PackageInfo
    {
        public string Id { get; set; }

        [DefaultValue(PackageManifest.LATEST_TAG)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Tag { get; set; }
        public string Version { get; set; }
        public string ManifestPath { get; set; }
        public bool Selected { get; set; }

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