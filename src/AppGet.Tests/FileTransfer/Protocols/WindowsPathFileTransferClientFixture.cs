using AppGet.FileTransfer.Protocols;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.FileTransfer.Protocols
{
    [TestFixture]
    public class WindowsPathFileTransferClientFixture : TestBase<WindowsPathFileTransferClient>
    {
        [TestCase("c:\\windows\\file.ext", "file.ext")]
        public void get_filename_from_path(string path, string fileName)
        {
            Subject.GetFileName(path).Should().Be(fileName);
        }
    }
}