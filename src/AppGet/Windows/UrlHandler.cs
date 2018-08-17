using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace AppGet.Windows
{
    public static class UrlHandler
    {
        private const string PROTOCOL = "appget";
        private const string PROTOCOL_HANDLER = "url.appget";

        private const string COMPANY_NAME = "AppGet";
        private const string PRODUCT_NAME = "AppGet";

        private static readonly string PATH = Process.GetCurrentProcess().MainModule.FileName;
        private static readonly string Launch = $"\"{PATH}\" \"%1\"";

        public static void Register()
        {
            RegisterWin7();

            if (Environment.OSVersion.Version >= new Version(6, 2, 9200, 0))
            {
                RegisterWin8();
            }
        }

        private static void RegisterWin7()
        {
            using (var protocolKey = Registry.ClassesRoot.CreateSubKey(PROTOCOL))
            {
                RegisterIcon(protocolKey);
                protocolKey.SetValue(null, $"URL:{PROTOCOL} Protocol");
                protocolKey.SetValue("URL Protocol", "");

                using (var commandKey = protocolKey.CreateSubKey(@"shell\open\command"))
                {
                    commandKey.SetValue(null, Launch);
                }
            }
        }

        private static void RegisterWin8()
        {

            using (var regKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes").CreateSubKey(PROTOCOL_HANDLER))
            {
                regKey.SetValue(null, PROTOCOL);

                RegisterIcon(regKey);

                regKey.CreateSubKey(@"shell\open\command").SetValue(null, Launch);
            }

            Registry.LocalMachine.CreateSubKey($@"SOFTWARE\{COMPANY_NAME}\{PRODUCT_NAME}\Capabilities\ApplicationDescription\URLAssociations")
                .SetValue(PROTOCOL, PROTOCOL_HANDLER);

            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\RegisteredApplications")
                .SetValue(PRODUCT_NAME, $@"SOFTWARE\{PRODUCT_NAME}\Capabilities");
        }

        private static void RegisterIcon(RegistryKey key)
        {
            key.CreateSubKey("DefaultIcon").SetValue(null, $"\"{PATH},1\"");
        }
    }
}
