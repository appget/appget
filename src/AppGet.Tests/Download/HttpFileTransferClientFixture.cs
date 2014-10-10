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
            Subject.OnStatusUpdates = state => Console.WriteLine(state.ToString());
            Subject.TransferFile("http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip", Path.Combine(temp, "LINQPad4.zip"));
            Directory.GetFiles(temp, "LINQPad4.zip").Should().NotBeEmpty();
        }
    }
}