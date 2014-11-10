using System;
using System.IO;
using AppGet.FileTransfer.Protocols;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Download
{
    [TestFixture]
    public class HttpFileTransferClientFixture : TestBase<HttpFileTransferClient>
    {
        [Test]
        public void should_download_file_using_correct_name()
        {
            var temp = Path.GetTempPath();
            Subject.OnStatusUpdated = state => Console.WriteLine(state.ToString());
            Subject.TransferFile("http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip", Path.Combine(temp, "LINQPad4.zip"));
            Directory.GetFiles(temp, "LINQPad4.zip").Should().NotBeEmpty();
        }

        [TestCase("http://server.com/dir/dir2/somefile.zip", "somefile.zip")]
        [TestCase("http://server.com/dir/dir2/somefile.rar", "somefile.rar")]
        [TestCase("http://server.com/dir/dir2/somefile.msi", "somefile.msi")]
        [TestCase("http://server.com/dir/dir2/somefile.exe?querystring=12", "somefile.exe")]
        [TestCase("http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip", "LINQPad4-AnyCPU.zip")]
        [TestCase("http://www.jtricks.com/download-unknown", "content.txt")]
        [TestCase("http://www.jtricks.com/download-text", "content.txt")]
        public void should_get_file_name_from_nameless_url(string url, string fileName)
        {
            WithRealHttp();
            Subject.GetFileName(url).Should().Be(fileName);
        }
    }
}