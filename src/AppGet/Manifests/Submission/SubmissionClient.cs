using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppGet.Http;
using AppGet.Manifest.Builder;
using AppGet.Manifest.Serialization;

namespace AppGet.Manifests.Submission
{
    public class SubmissionResponse
    {
        public string PullRequestUrl { get; set; }
        public string Branch { get; set; }
        public bool ExistingPullRequest { get; set; }
        public string Message { get; set; }

        public List<string> Errors { get; set; }
        public List<string> Notices { get; set; }
    }

    public interface ISubmissionClient
    {
        Task<SubmissionResponse> Submit(PackageManifestBuilder builder);
    }

    public class SubmissionClient : ISubmissionClient
    {
        private readonly IHttpClient _httpClient;

        public SubmissionClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SubmissionResponse> Submit(PackageManifestBuilder builder)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "https://octobot.appget.net/pr")
            {
                Content = new StringContent(Json.Serialize(builder), Encoding.UTF8, "application/json")
            };

            var resp = await _httpClient.SendAsync(req);
            return resp.Deserialize<SubmissionResponse>();
        }
    };
}