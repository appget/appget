using System;
using AppGet.FlightPlans;
using AppGet.Serialization;
using NUnit.Framework;

namespace AppGet.Tests.FlightPlans
{
    [TestFixture]
    public class FlightPlanFixture
    {
        [Test]
        public void print_sample_flight_plan()
        {
            var flightPlan = new FlightPlan
            {
                Id = "firefox",
                Version = "latest",
                Exe = new[] { "firefox.exe" },
       
                Installer = InstallerType.MSI,
                Packages = new[]
                {
                    new PackageSource
                    {
                        Source = "http://mozilla.com/firefox.exe",
                        Sha256 = "730e109bd7a8a32b1cb9d9a09aa2325d2430587ddbc0c38bad911525",
                        Architecture = ArchitectureType.x86,
                        MinWindowsVersion = new Version(5, 1),
                        MaxWindowsVersion = new Version(6, 4),
                    },
                    new PackageSource
                    {
                        Source = "http://mozilla.com/firefox_x64.exe",
                        Sha256 = "38b060a751ac96384cd9327eb1b1e36a21fdb71114be07434c0cc7bf",
                        Architecture = ArchitectureType.x64,
                        MinWindowsVersion = new Version(5, 1),
                        MaxWindowsVersion = new Version(6, 4),
                    }
                }
            };


            Console.WriteLine(Yaml.Serialize(flightPlan));
        }
    }
}