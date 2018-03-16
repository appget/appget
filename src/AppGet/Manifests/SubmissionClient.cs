using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppGet.Http;
using AppGet.Serialization;

namespace AppGet.Manifests
{

    public class SubmissionClient
    {
        private readonly IHttpClient _httpClient;

        public SubmissionClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Submit(PackageManifest manifest, string fileName)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, $"https://fn.appget.net/api/pullrequest/{fileName}");
            req.Content = new StringContent(Yaml.Serialize(manifest), Encoding.UTF8, "application/yaml");

            var c = await _httpClient.Send(req);

            return;
        }

    }
}
