using System.IO;
using AppGet.FileTransfer;
using AppGet.FileTransfer.Protocols;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.FileTransfer.Protocols
{
    [TestFixture]
    public class HttpFileTransferClientFixture : TestBase<HttpFileTransferClient>
    {
        [SetUp]
        public void Setup()
        {
            WithRealHttp();
        }

        [TestCase("https://www.fosshub.com/ConEmu.html/ConEmuPack.161206.7z")]
        public void should_throw_on_html_download_file(string url)
        {
            Assert.Throws<InvalidDownloadUrlException>(() =>
            {
                Subject.TransferFile(url, Path.Combine(Path.GetTempPath(), Subject.GetFileName(url)));
            });
        }

        [Test]
        public void should_download_file_using_correct_name()
        {
            const string url = "http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip";
            Subject.GetFileName(url).Should().Be("LINQPad4-AnyCPU.zip");
        }

        [TestCase("https://dl.pstmn.io/download/latest/win64")]
        public void should_get_file_name(string url)
        {
            Subject.GetFileName(url).Should().EndWith(".exe");
        }

        [TestCase("http://server.com/dir/dir2/somefile.zip", "somefile.zip")]
        [TestCase("http://server.com/dir/dir2/somefile.7z", "somefile.7z")]
        [TestCase("http://server.com/dir/dir2/somefile.rar", "somefile.rar")]
        [TestCase("http://server.com/dir/dir2/somefile.msi", "somefile.msi")]
        [TestCase("http://server.com/dir/dir2/somefile.exe?querystring=12", "somefile.exe")]
        [TestCase("http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip", "LINQPad4-AnyCPU.zip")]
        [TestCase("http://www.jtricks.com/download-unknown", "content.txt")]
        [TestCase("http://www.jtricks.com/download-text", "content.txt")]
        public void should_get_file_name_from_nameless_url(string url, string fileName)
        {
            Subject.GetFileName(url).Should().Be(fileName);
        }
    }
}