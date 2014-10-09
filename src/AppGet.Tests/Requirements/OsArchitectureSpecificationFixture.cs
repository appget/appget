using System;
using AppGet.Environment;
using AppGet.FlightPlans;
using AppGet.Requirements.Specifications;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Requirements
{
    [TestFixture]
    public class MaxOsVersionSpecificationFixture : TestBase<MaxOsVersionSpecification>
    {
        [Test]
        public void should_be_true_when_max_has_not_been_set()
        {
            Mocker.GetMock<IEnvironmentProxy>()
                  .SetupGet(s => s.OSVersion)
                  .Returns(new OperatingSystem(PlatformID.Win32NT, new Version(6, 3, 9600, 0)));

            Subject.IsRequirementSatisfied(new PackageSource()).Should().BeTrue();
        }

        [Test]
        public void should_be_true_when_OS_is_less_than_max()
        {
            Mocker.GetMock<IEnvironmentProxy>()
                  .SetupGet(s => s.OSVersion)
                  .Returns(new OperatingSystem(PlatformID.Win32NT, new Version(6, 3, 9600, 0)));

            Subject.IsRequirementSatisfied(new PackageSource
                                           {
                                               MaxWindowsVersion = new Version(6, 4)
                                           }).Should().BeTrue();
        }

        [Test]
        public void should_be_false_when_OS_is_greater_than_max()
        {
            Mocker.GetMock<IEnvironmentProxy>()
                  .SetupGet(s => s.OSVersion)
                  .Returns(new OperatingSystem(PlatformID.Win32NT, new Version(6, 3, 9600, 0)));

            Subject.IsRequirementSatisfied(new PackageSource
                                           {
                                               MaxWindowsVersion = new Version(6, 0)
                                           }).Should().BeFalse();
        }
    }
}
