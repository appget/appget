using System.Diagnostics;
using System.Linq;
using AppGet.Windows;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Windows
{
    [TestFixture]
    public class ManagementObjectFixture : TestBase<ManagementObject>
    {
        [Test]
        public void get_result()
        {
            var processes = Subject.QueryProcess(@"ExecutablePath LIKE 'C:\\program Files\\%'").ToList();

            processes.Should().NotBeEmpty();
            processes.Should().OnlyContain(c => c > 0);

            foreach (var p in processes)
            {
                Process.GetProcessById(p).Should().NotBeNull();
            }
        }

        [Test]
        public void get_by_path()
        {
            var processes = Subject.GetProcessByPath(@"C:\program Files\").ToList();

            processes.Should().NotBeEmpty();
            processes.Should().OnlyContain(c => c > 0);

            foreach (var p in processes)
            {
                Process.GetProcessById(p).Should().NotBeNull();
            }
        }
    }
}