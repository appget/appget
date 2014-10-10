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
                Id = "linqpad",
                Version = "4.51.03",
                Exe = new[] { "LINQPad.exe" },
                ApplicationUrl ="http://www.linqpad.net/",
                Installer = InstallerType.Zip,
                Packages = new[]
                {
                    new PackageSource
                    {
                        Source = "http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip",
                        Architecture = ArchitectureType.Any
                    }
                }
            };


            Console.WriteLine(Yaml.Serialize(flightPlan));
        }
    }
}