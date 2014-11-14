using System.IO;
using AppGet.Processes;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Processes
{
    [TestFixture]
    public class FileInUseFixture
    {
       /* [TestCase]
        public void should_get_locking_process()
        {
            var tempFile1 = Path.GetTempFileName();
            var tempFile2 = Path.GetTempFileName();
            File.OpenWrite(tempFile1);
            File.OpenWrite(tempFile2);
            var processes = FileInUse.GetLockers(tempFile1, tempFile2);
            processes.Should().NotBeNull();
            processes.Should().NotBeEmpty();
            processes.Should().HaveCount(1);
        } */
    }
}