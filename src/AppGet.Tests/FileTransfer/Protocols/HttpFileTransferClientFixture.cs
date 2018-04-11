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
            Assert.ThrowsAsync<InvalidDownloadUrlException>(() =>
            {
                Subject.TransferFile(url, Path.Combine(Path.GetTempPath(), Subject.GetFileName(url)));
                return null;
            });
        }

        [Test]
        public void should_download_file_using_correct_name()
        {
            const string url = "http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip";
            var fileName = Subject.GetFileName(url);
            fileName.Should().Be("LINQPad4-AnyCPU.zip");
        }

        [TestCase("https://dl.pstmn.io/download/latest/win64")]
        public void should_get_file_name(string url)
        {
            var fileName = Subject.GetFileName(url);
            fileName.Should().EndWith(".exe");
        }

        [TestCase("http://server.com/dir/dir2/somefile.zip", "somefile.zip")]
        [TestCase("http://server.com/dir/dir2/somefile.7z", "somefile.7z")]
        [TestCase("http://server.com/dir/dir2/somefile.rar", "somefile.rar")]
        [TestCase("http://server.com/dir/dir2/somefile.msi", "somefile.msi")]
        [TestCase("https://download.sublimetext.com/Sublime Text Build 3143 x64 Setup.exe", "Sublime Text Build 3143 x64 Setup.exe")]
        [TestCase("http://server.com/dir/dir2/somefile.exe?querystring=12", "somefile.exe")]
        [TestCase("http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip", "LINQPad4-AnyCPU.zip")]
        [TestCase("http://www.jtricks.com/download-unknown", "content.txt")]
        [TestCase("http://www.jtricks.com/download-text", "content.txt")]
        public void should_get_file_name_from_nameless_url(string url, string expected)
        {
            var fileName = Subject.GetFileName(url);
            fileName.Should().Be(expected);
        }

        [Test]
        public void read_file()
        {
            var text = Subject.ReadString("https://raw.githubusercontent.com/appget/packages/master/LICENSE");

            text.Should().NotBeNullOrWhiteSpace();
        }
    }
}