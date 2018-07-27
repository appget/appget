using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppGet.WindowsInstaller
{
    public class WindowsInstallerRecord
    {
        public bool Is64 { get; set; }
        public string Id { get; set; }
        public bool IsUpgradeNode { get; set; }
        public string Hive { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> Values { get; set; }
    }
}