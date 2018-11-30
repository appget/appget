
using AppGet.Installers;
using AppGet.Windows.WindowsInstaller;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Installers
{
    [TestFixture]
    public class DiffGeneratorFixture
    {
        [Test]
        public void diff_should_return_empty_for_same_list()
        {
            var installerClient = new WindowsInstallerClient();

            var diffGenerator = new DiffGenerator<WindowsInstallerRecord>(installerClient.GetRecords(), installerClient.GetRecords(), c => $"{c.Is64}_{c.Id}");


            diffGenerator.Added().Should().BeEmpty();
            diffGenerator.Removed().Should().BeEmpty();
            diffGenerator.Updated().Should().BeEmpty();
        }


        [Test]
        public void diff_should_show_added()
        {
            var installerClient = new WindowsInstallerClient();

            var before = installerClient.GetRecords();
            var after = installerClient.GetRecords();

            var newRecord = new WindowsInstallerRecord
            {
                Id = "new",
            };

            after.Add(newRecord);

            var diffGenerator = new DiffGenerator<WindowsInstallerRecord>(before, after, c => $"{c.Is64}_{c.Id}");


            diffGenerator.Added().Should().OnlyContain(c => c == newRecord);
            diffGenerator.Removed().Should().BeEmpty();
            diffGenerator.Updated().Should().BeEmpty();
        }


        [Test]
        public void diff_should_show_updated()
        {
            var installerClient = new WindowsInstallerClient();

            var before = installerClient.GetRecords();
            var after = installerClient.GetRecords();

            after[20].Values["foo"] = "bar";

            var diffGenerator = new DiffGenerator<WindowsInstallerRecord>(before, after, c => $"{c.Is64}_{c.Id}");


            diffGenerator.Added().Should().BeEmpty();
            diffGenerator.Removed().Should().BeEmpty();
            diffGenerator.Updated().Should().OnlyContain(c => c.Values["foo"].ToString() == "bar");
        }


        [Test]
        public void diff_should_show_removed()
        {
            var installerClient = new WindowsInstallerClient();

            var before = installerClient.GetRecords();
            var after = installerClient.GetRecords();

            after.RemoveAt(20);

            var diffGenerator = new DiffGenerator<WindowsInstallerRecord>(before, after, c => $"{c.Is64}_{c.Id}");


            diffGenerator.Added().Should().BeEmpty();
            diffGenerator.Removed().Should().HaveCount(1);
            diffGenerator.Updated().Should().BeEmpty();
        }
    }
}