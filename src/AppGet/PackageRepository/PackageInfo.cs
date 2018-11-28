using AppGet.Manifest;
using Newtonsoft.Json;

namespace AppGet.PackageRepository
{
    public class PackageInfo : PackageManifest
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string ManifestPath { get; set; }

        public bool Selected { get; set; }

        public override string ToString()
        {
            var text = $"{Id}";
            if (!IsLatest)
            {
                text += $":{Tag}";
            }

            return text;
        }
    }
}