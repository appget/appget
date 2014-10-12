using System;
using System.Collections.Generic;
using AppGet.FlightPlans;
using AppGet.HostSystem;
using AppGet.Packages;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Packages
{
    [TestFixture]
    public class FindInstallerFixture : TestBase<FindInstaller>
    {
        private List<Installer> _packages;
            
        [SetUp]
        public void Setup()
        {
            _packages = new List<Installer>();

            Mocker.GetMock<IEnvironmentProxy>()
                  .SetupGet(s => s.Is64BitOperatingSystem)
                  .Returns(false);
        }

        private void GivenX64Package()
        {
            _packages.Add(new Installer { Architecture = ArchitectureType.x64 });
        }

        private void GivenX86Package()
        {
            _packages.Add(new Installer { Architecture = ArchitectureType.x86 });
        }

        private void GivenAnyPackage()
        {
            _packages.Add(new Installer { Architecture = ArchitectureType.Any });
        }

        private void GivenX64OS()
        {
            Mocker.GetMock<IEnvironmentProxy>()
                  .SetupGet(s => s.Is64BitOperatingSystem)
                  .Returns(true);
        }

        [Test]
        public void should_return_x64_package_when_os_is_x64_and_x64_package_is_available()
        {
            GivenX64OS();
            GivenX64Package();

            Subject.GetPackage(_packages).Architecture.Should().Be(ArchitectureType.x64);
        }

        [Test]
        public void should_return_x86_package_when_os_is_x86_and_x86_package_is_available()
        {
            GivenX86Package();

            Subject.GetPackage(_packages).Architecture.Should().Be(ArchitectureType.x86);
        }

        [Test]
        public void should_return_x86_package_when_os_is_x64_and_x86_package_is_available()
        {
            GivenX86Package();

            Subject.GetPackage(_packages).Architecture.Should().Be(ArchitectureType.x86);
        }

        [Test]
        public void should_return_any_package_when_os_is_x64_and_any_package_is_available()
        {
            GivenAnyPackage();

            Subject.GetPackage(_packages).Architecture.Should().Be(ArchitectureType.Any);
        }

        [Test]
        public void should_return_any_package_when_os_is_x86_and_any_package_is_available()
        {
            GivenAnyPackage();

            Subject.GetPackage(_packages).Architecture.Should().Be(ArchitectureType.Any);
        }

        [Test]
        public void should_throw_when_acceptable_package_is_not_available()
        {
            Assert.Throws<NotSupportedException>(() => Subject.GetPackage(_packages));
        }
    }
}
