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

        public UserAgentBuilder(IOsInfo osInfo)
        {
            var bits = osInfo.Is64BitOperatingSystem ? "64" : "32";

            _userAgent = $"AppGet/{BuildInfo.Version} ({ osInfo.Name} {osInfo.Version}; {bits})";
            _userAgentSimplified = $"AppGet/{BuildInfo.Version.ToString(2)}";
        }
    }
}