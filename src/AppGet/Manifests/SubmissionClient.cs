using System.Net.Http;
using System.Text;
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

        public  void Submit(PackageManifest manifest, string fileName)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, $"https://fn.appget.net/api/pullrequest/{fileName}");
            req.Content = new StringContent(Yaml.Serialize(manifest), Encoding.UTF8, "application/yaml");

            var c =  _httpClient.Send(req);

            return;
        }

    }
}
