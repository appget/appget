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
            var bits = envInfo.Is64BitOperatingSystem ? "Win64; x64;" : "Win32; x86;";

            _userAgentSimplified = $"{BuildInfo.ProductName}/{BuildInfo.AppVersion}";
            _userAgent = $"{_userAgentSimplified} ({envInfo.Name} {envInfo.WindowsVersion}; {bits})";
        }
    }
}