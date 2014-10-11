using System.IO;
using AppGet.Processes;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Processes
{
    [TestFixture]
    public class HandleFixture
    {
        [TestCase]
        public void should_get_locking_process()
        {
            var tempFile = Path.GetTempFileName();
            File.OpenWrite(tempFile);
            var processes = Handle.GetLockers(tempFile);
            processes.Should().NotBeNull();
        }
    }
}