using System;
using System.Threading.Tasks;
using AppGet.Http;

namespace AppGet.CreatePackage
{
    public interface IXRayClient
    {
        Task<PackageManifestBuilder> GetBuilderAsync(Uri uri);
    }

    public class XRayClient : IXRayClient
    {
        private readonly IHttpClient _httpClient;

        public XRayClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PackageManifestBuilder> GetBuilderAsync(Uri uri)
        {
            var resp = await _httpClient.Get($"https://fn.appget.net/api/xray?url={uri}");
            var builder = await resp.AsResource<PackageManifestBuilder>();
            return builder;
        }

    }
}
