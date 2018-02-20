using System;
using AppGet.CreatePackage.Parsers;
using AppGet.Manifests;
using NUnit.Framework;

namespace AppGet.Tests.CreatePackage.Parsers
{
    [TestFixture]
    public class ArchitectureParserFixture
    {
        [TestCase("https://download.sublimetext.com/Sublime Text Build 3143 x64 Setup.exe", ExpectedResult = ArchitectureTypes.x64)]
        [TestCase("https://notepad-plus-plus.org/repository/7.x/7.5.4/npp.7.5.4.Installer.exe", ExpectedResult = ArchitectureTypes.x86)]
        [TestCase("https://github.com/webtorrent/webtorrent-desktop/releases/download/v0.19.0/WebTorrentSetup-v0.19.0-ia32.exe", ExpectedResult = ArchitectureTypes.x86)]
        [TestCase("https://github.com/webtorrent/webtorrent-desktop/3.65.4364", ExpectedResult = ArchitectureTypes.x86)]
        [TestCase("https://notepad-plus-plus.org/repository/7.x/7.5.4/npp.7.5.4.Installer.exe?type=64", ExpectedResult = ArchitectureTypes.x64)]
        [TestCase("https://notepad-plus-plus.org/repository/7.x/7.5.4/npp.7.5.4.Installer.x64.exe", ExpectedResult = ArchitectureTypes.x64)]
        [TestCase("http://download.videolan.org/pub/videolan/vlc/3.0.0/win64/vlc-3.0.0-win64.exe", ExpectedResult = ArchitectureTypes.x64)]
        [TestCase("https://dl.pstmn.io/download/latest/win64", ExpectedResult = ArchitectureTypes.x64)]
        public ArchitectureTypes should_proper_arch(string url)
        {
            return ArchitectureParser.Parse(new Uri(url));
        }
    }
}