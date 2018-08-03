using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppGet.Http;
using AppGet.Manifest.Serialization;
using AppGet.Windows.WindowsInstaller;

namespace AppGet.Update
{
    public class NovoClient
    {
        private readonly IHttpClient _httpClient;

        public NovoClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PackageUpdate>> GetUpdates(IEnumerable<WindowsInstallerRecord> records)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://appget-novo.azurewebsites.net/updates")
            {
                Content = new StringContent(Json.Serialize(records), Encoding.Default, "application/json")
            };

            var resp = await _httpClient.SendAsync(request);


            return resp.Deserialize<List<PackageUpdate>>();
        }


        public async Task<List<PackageUpdate>> GetUpdate(IEnumerable<WindowsInstallerRecord> records, string packageId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:7071/updates?pkg={packageId}")
            {
                Content = new StringContent(Json.Serialize(records), Encoding.Default, "application/json")
            };

            var resp = await _httpClient.SendAsync(request);

            return resp.Deserialize<List<PackageUpdate>>();
        }
    }
}
