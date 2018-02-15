using AppGet.Http;
using NLog;

namespace AppGet.Update
{
    public class AppGetUpdateService
    {
        private readonly IHttpClient _httpClient;

        public AppGetUpdateService(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
        }

    }
}
