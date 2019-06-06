using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AppGet.Tests
{
    public class HttpBinResponse
    {
        public Dictionary<string, string> Args { get; set; }
        public string Data { get; set; }
        public Dictionary<string, string> Files { get; set; }
        public Dictionary<string, string> Form { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public JObject Json { get; set; }
        public string Origin { get; set; }
        public string Url { get; set; }
    }
}