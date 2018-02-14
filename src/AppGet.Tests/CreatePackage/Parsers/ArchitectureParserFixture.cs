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
        [TestCase("https://notepad-plus-plus.org/repository/7.x/7.5.4/npp.7.5.4.Installer.exe?type=64", ExpectedResult = ArchitectureTypes.x64)]
        [TestCase("https://notepad-plus-plus.org/repository/7.x/7.5.4/npp.7.5.4.Installer.x64.exe", ExpectedResult = ArchitectureTypes.x64)]
        public ArchitectureTypes should_proper_arch(string url)
        {
            return ArchitectureParser.Parse(new Uri(url));
        }
    }
}