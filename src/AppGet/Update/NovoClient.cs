using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AppGet.Http;
using AppGet.Windows.WindowsInstaller;

namespace AppGet.Update
{
    public class NovoClient
    {
        private readonly IHttpClient _httpClient;

        private const string NOVO_ROOT = "https://novo.appget.net";

        public NovoClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PackageUpdate>> GetUpdates(IEnumerable<WindowsInstallerRecord> records)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{NOVO_ROOT}/updates")
            {
                Content = new JsonContent(records)
            };

            var resp = await _httpClient.SendAsync(request, TimeSpan.FromSeconds(60));


            return await resp.Deserialize<List<PackageUpdate>>();
        }


        public async Task<List<PackageUpdate>> GetUpdate(IEnumerable<WindowsInstallerRecord> records, string packageId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{NOVO_ROOT}/updates?pkg={packageId}")
            {
                Content = new JsonContent(records)
            };

            var resp = await _httpClient.SendAsync(request, TimeSpan.FromSeconds(60));

            return await resp.Deserialize<List<PackageUpdate>>();
        }

        public async Task<List<UninstallData>> GetUninstall(IEnumerable<WindowsInstallerRecord> records, string packageId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{NOVO_ROOT}/uninstall?pkg={packageId}")
            {
                Content = new JsonContent(records)
            };

            var resp = await _httpClient.SendAsync(request, TimeSpan.FromSeconds(60));

            return await resp.Deserialize<List<UninstallData>>();
        }
    }
}
