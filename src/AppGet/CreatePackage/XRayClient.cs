using System;
using System.Net;
using System.Threading.Tasks;
using AppGet.Http;

namespace AppGet.CreatePackage
{
    public interface IXRayClient
    {
        Task<PackageManifestBuilder> GetBuilder(Uri uri, string name = null);
        Task<InstallerBuilder> GetInstallerBuilder(Uri uri);
    }

    public class XRayClient : IXRayClient
    {
        private readonly IHttpClient _httpClient;

        public XRayClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PackageManifestBuilder> GetBuilder(Uri uri, string name = null)
        {

            //             var url = $"http://localhost:7071/api/xray?url={WebUtility.UrlEncode(uri.ToString())}";
            var url = $"https://fn.appget.net/api/xray?url={WebUtility.UrlEncode(uri.ToString())}";


            if (name != null)
            {
                url += $"&id={WebUtility.UrlEncode(name)}";
            }

            var resp = await _httpClient.Get(url);
            var builder = await resp.AsResource<PackageManifestBuilder>();
            return builder;
        }

        public async Task<InstallerBuilder> GetInstallerBuilder(Uri uri)
        {

            // var url = $"http://localhost:7071/api/xray/installer?url={WebUtility.UrlEncode(uri.ToString())}";
            var url = $"https://fn.appget.net/api/xray/installer?url={WebUtility.UrlEncode(uri.ToString())}";

            var resp = await _httpClient.Get(url);
            var builder = await resp.AsResource<InstallerBuilder>();
            return builder;
        }

    }
}
