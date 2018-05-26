using System;
using System.Net;
using AppGet.Http;
using AppGet.Manifest.Builder;
using NLog;

namespace AppGet.CreatePackage
{
    public interface IXRayClient
    {
        PackageManifestBuilder GetBuilder(Uri uri, string name = null);
        InstallerBuilder GetInstallerBuilder(Uri uri);
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

        public PackageManifestBuilder GetBuilder(Uri uri, string name = null)
        {
            var url = $"https://api.appget.net/xray?url={WebUtility.UrlEncode(uri.ToString())}";

            if (name != null)
            {
                url += $"&id={WebUtility.UrlEncode(name)}";
            }

            _logger.Info($"Requesting analysis on {uri}");
            _logger.Info("This could take a couple of minutes...");

            var resp = _httpClient.Get(new Uri(url));
            var builder = resp.AsResource<PackageManifestBuilder>();

            _logger.Debug($"Received results for {uri}");

            return builder;
        }

        public InstallerBuilder GetInstallerBuilder(Uri uri)
        {
            var url = $"https://api.appget.net/xray/installer?url={WebUtility.UrlEncode(uri.ToString())}";

            var resp = _httpClient.Get(new Uri(url));
            var builder = resp.AsResource<InstallerBuilder>();

            return builder;
        }
    }
}