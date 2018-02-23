using System.Threading.Tasks;
using AppGet.FileTransfer.Protocols;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.FileTransfer.Protocols
{
    [TestFixture]
    public class WindowsPathFileTransferClientFixture : TestBase<WindowsPathFileTransferClient>
    {
        [TestCase("c:\\windows\\file.ext", "file.ext")]
        public async Task get_filename_from_path(string path, string fileName)
        {
            var result = await Subject.GetFileName(path);
            result.Should().Be(fileName);
        }
    }
}