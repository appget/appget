using System;
using System.IO;
using AppGet.Download;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Download
{
    [TestFixture]
    public class HttpDownloadClientFixture : TestBase<HttpDownloadClient>
    {
        [Test]
        public void should_download_file_using_correct_name()
        {
            var temp = Path.GetTempPath();
            Subject.OnStatusUpdates = state => Console.WriteLine(state.ToString());
            Subject.DownloadFile("http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip", Path.Combine(temp, "LINQPad4.zip"));
            Directory.GetFiles(temp, "LINQPad4.zip").Should().NotBeEmpty();
        }
    }
}