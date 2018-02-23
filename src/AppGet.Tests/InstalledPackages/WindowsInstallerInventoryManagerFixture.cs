using System;
using System.Linq;
using AppGet.InstalledPackages;
using AppGet.Serialization;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.InstalledPackages
{
    public class WindowsInstallerInventoryManagerFixture : TestBase<WindowsInstallerInventoryManager>
    {
        [Test]
        public void should_get_uninstall_records()
        {
            var records = Subject.GetInstalledApplications().OrderBy(c => c.Name).ToList();

            //            var sq = records.Where(c => c.InstallMethod == InstallMethodTypes.Squirrel).ToList();


            foreach (var r in records.OrderBy(c => c.Version))
            {


                Console.WriteLine($"{r.Version}  <|> {r.Name}");
                //                var c = DateTime.TryParse(r, out var d);
                //                if (c)
                //                {
                //                    Console.WriteLine(d);
                //                }
                //                else
                //                {
                //                    Console.WriteLine("bad: "+ r);
                //                }
            }

            records.Should().NotBeEmpty();
        }

        [TestCase("VLC", Category = "Local")]
        [TestCase("slack", Category = "Local")]
        public void should_find_install_record(string name)
        {
            var apps = Subject.GetInstalledApplications(name);
            apps.Should().HaveCount(1);
        }
    }
}