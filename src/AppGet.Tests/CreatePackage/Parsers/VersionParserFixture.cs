using System;
using AppGet.CreatePackage.Parsers;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.CreatePackage.Parsers
{
    [TestFixture]
    public class VersionParserFixture
    {
        [TestCase("https://download.sublimetext.com/Sublime Text Build 3143 x64 Setup.exe", ExpectedResult = "3143")]
        [TestCase("https://notepad-plus-plus.org/repository/7.x/7.5.4/npp.7.5.4.Installer.exe", ExpectedResult = "7.5.4")]
        [TestCase("https://nodejs.org/dist/v8.9.4/node-v8.9.4-x86.msi", ExpectedResult = "8.9.4")]
        [TestCase("https://nodejs.org/dist/v8.9.4/node-v8.9.4.400-x86.msi", ExpectedResult = "8.9.4.400")]
        [TestCase("https://download.piriform.com/ccsetup500.exe", ExpectedResult = "500")]
        [TestCase("http://www.7-zip.org/a/7z1801-x64.exe", ExpectedResult = "1801")]
        [TestCase("https://github.com/gitextensions/gitextensions/releases/download/v2.51/GitExtensions-2.51-Setup.msi", ExpectedResult = "2.51")]
        public string should_parse_version(string url)
        {
            return VersionParser.Parse(new Uri(url));
        }


        [TestCase("https://download.sublimetext.com/Sublime Text Build x64 Setup.exe")]
        [TestCase("https://g.com/win7.exe")]
        [TestCase("https://g.com/windows10.exe")]
        [TestCase("https://g.com/g_i386.exe")]
        [TestCase("https://download.piriform.com/ccsetup.exe")]
        [TestCase("http://www.7-zip.org/a/x64.exe")]
        [TestCase("https://download.mozilla.org/?product=firefox-latest-ssl&os=win64&lang=en-US")]
        public void should_not_return_Version(string url)
        {
            VersionParser.Parse(new Uri(url)).Should().BeNull();
        }
    }
}