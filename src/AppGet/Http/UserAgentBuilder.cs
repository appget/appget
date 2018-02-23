using AppGet.HostSystem;

namespace AppGet.Http
{
    public interface IUserAgentBuilder
    {
        string GetUserAgent(bool simplified = false);
    }

    public class UserAgentBuilder : IUserAgentBuilder
    {
        private readonly string _userAgentSimplified;
        private readonly string _userAgent;

        public string GetUserAgent(bool simplified)
        {
            return simplified ? _userAgentSimplified : _userAgent;
        }

        public UserAgentBuilder(IEnvInfo envInfo)
        {
            var bits = envInfo.Is64BitOperatingSystem ? "64" : "32";

            _userAgent = $"AppGet/{BuildInfo.Version} ({ envInfo.Name} {envInfo.Version}; {bits})";
            _userAgentSimplified = $"AppGet/{BuildInfo.Version.ToString(2)}";
        }
    }
}