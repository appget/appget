using System.Collections.Generic;
using System.Linq;
using AppGet.Installers.UninstallerWhisperer;
using AppGet.Manifest;
using AppGet.Update;
using AppGet.Windows.WindowsInstaller;
using Colorful;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Installers.UninstallerWhisperer
{

    public class TestUninstaller : UninstallerBase
    {
        public override InstallMethodTypes InstallMethod { get; }
        public override string InteractiveArgs { get; }
        public override string PassiveArgs { get; }
        public override string SilentArgs { get; }
        public override string LogArgs { get; }
    }

    [TestFixture]
    public class UninstallerBaseFixture : TestBase<TestUninstaller>
    {
        [TestCaseSource(nameof(GetUninstallStrings))]
        public void should_parse_uninstaller_process(string uninstallString)
        {
            var keys = new Dictionary<string, string>
            {
                {
                    "UninstallString", uninstallString
                }
            };

            Subject.InitUninstaller(keys, new UninstallData());

            var path = Subject.GetProcessPath();


            Console.WriteLine(path);

            path.Should().EndWith(".exe");
        }

        private static IEnumerable<string> GetUninstallStrings()
        {
            var installerClient = new WindowsInstallerClient();
            return installerClient.GetRecords()
                .Where(c => c.Values.ContainsKey("UninstallString"))
                .Select(c => c.Values["UninstallString"].ToString())
                .Where(c => !c.ToLower().Contains("msiexec"));
        }

    }
}