using System;
using AppGet.HostSystem;
using AppGet.Installers.Requirements.Specifications;
using AppGet.Manifest;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Requirements.Specifications
{
    [TestFixture]
    public class MinOsVersionSpecificationFixture : TestBase<MinOsVersionSpecification>
    {
        [Test]
        public void should_be_true_when_min_has_not_been_set()
        {
            Mocker.GetMock<IEnvInfo>()
                  .SetupGet(s => s.WindowsVersion)
                  .Returns(new Version(1, 0));

            Subject.IsRequirementSatisfied(new Installer()).Success.Should().BeTrue();
        }

        [Test]
        public void should_be_true_when_OS_is_greater_than_min()
        {
            Mocker.GetMock<IEnvInfo>()
                  .SetupGet(s => s.WindowsVersion)
                  .Returns(new Version(10, 0));

            Subject.IsRequirementSatisfied(new Installer
            {
                MinWindowsVersion = new Version(6, 1)
            }).Success.Should().BeTrue();
        }

        [Test]
        public void should_be_true_when_OS_is_same_as_min()
        {
            Mocker.GetMock<IEnvInfo>()
                .SetupGet(s => s.WindowsVersion)
                .Returns(new Version(10, 0));

            Subject.IsRequirementSatisfied(new Installer
            {
                MinWindowsVersion = new Version(10, 0)
            }).Success.Should().BeTrue();
        }

        [Test]
        public void should_be_false_when_OS_is_less_than_min()
        {
            Mocker.GetMock<IEnvInfo>()
                  .SetupGet(s => s.WindowsVersion)
                .Returns(new Version(6, 0));

            Subject.IsRequirementSatisfied(new Installer
            {
                MinWindowsVersion = new Version(10, 0)
            }).Success.Should().BeFalse();
        }
    }
}
