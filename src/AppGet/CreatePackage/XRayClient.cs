using System;
using System.Net;
using System.Threading.Tasks;
using AppGet.Http;
using NLog;

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
        private readonly Logger _logger;

        public XRayClient(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PackageManifestBuilder> GetBuilder(Uri uri, string name = null)
        {
            var url = $"https://fn.appget.net/api/xray?url={WebUtility.UrlEncode(uri.ToString())}";


            if (name != null)
            {
                url += $"&id={WebUtility.UrlEncode(name)}";
            }

            _logger.Info($"Requesting analysis on {uri}. This might take a couple of minutes.");

            var resp = await _httpClient.Get(new Uri(url));
            var builder = await resp.AsResource<PackageManifestBuilder>();

            _logger.Info($"Received results for {uri}");

            return builder;
        }

        public async Task<InstallerBuilder> GetInstallerBuilder(Uri uri)
        {
            var url = $"https://fn.appget.net/api/xray/installer?url={WebUtility.UrlEncode(uri.ToString())}";

            var resp = await _httpClient.Get(new Uri(url));
            var builder = await resp.AsResource<InstallerBuilder>();
            return builder;
        }
    }
}
