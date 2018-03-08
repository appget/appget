using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppGet.Http;

namespace AppGet.CreatePackage
{
    public class XRayClient
    {
        private readonly IHttpClient _httpClient;

        public XRayClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PackageManifestBuilder> GetBuilderAsync(Uri uri)
        {
            var resp = await _httpClient.Get($"https://appget-fn.azurewebsites.net/api/xray?url={uri}");
            var builder = await resp.AsResource<PackageManifestBuilder>();
            return builder;
        }

    }
}
