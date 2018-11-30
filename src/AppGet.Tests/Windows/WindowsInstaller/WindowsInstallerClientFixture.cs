using System;
using System.Linq;
using AppGet.Manifest.Serialization;
using AppGet.Windows.WindowsInstaller;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AppGet.Tests.Windows.WindowsInstaller
{
    [TestFixture]
    public class WindowsInstallerClientFixture : TestBase<WindowsInstallerClient>
    {
        [Test]
        public void should_get_records()
        {
            var keys = Subject.GetRecords().OrderBy(c => c.Id).ToList();

            Console.WriteLine(Json.Serialize(keys, Formatting.Indented));


            keys.Should().HaveCountGreaterThan(100);

            keys.Should().Contain(c => c.IsUpgradeNode);
            keys.Should().Contain(c => !c.IsUpgradeNode);

            keys.Should().Contain(c => c.Is64);
            keys.Should().Contain(c => !c.Is64);

            keys.Should().OnlyHaveUniqueItems(c => c.Id + c.Is64);

            keys.Should().OnlyContain(c => c.Values.Any());
        }


        [Test]
        public void should_get_key_by_id()
        {
            var installerRecords = Subject.GetRecords().Where(c => !c.IsUpgradeNode).ToList();



            var g = installerRecords.GroupBy(c => c.Id).ToList();

            var pp = g.Where(c => c.Count() != 1).ToList();
            var cc = g.Where(c => c.All(d => d.Is64)).ToList();
            var c2 = g.Where(c => c.All(d => !d.Is64)).ToList();

            foreach (var record in installerRecords)
            {
                var keys = Subject.GetKey(record.Id);
                keys.Should().NotBeNull();
            }

        }
    }
}