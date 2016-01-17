using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Requirements.Specifications;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Requirements
{
    [TestFixture]
    public class MinOsVersionSpecificationFixture : TestBase<MinOsVersionSpecification>
    {
        [Test]
        public void should_be_true_when_min_has_not_been_set()
        {
            Mocker.GetMock<IEnvironmentProxy>()
                  .SetupGet(s => s.WindowsVersion)
                  .Returns(WindowsVersion.Eight);

            Subject.IsRequirementSatisfied(new Installer()).Success.Should().BeTrue();
        }

        [Test]
        public void should_be_true_when_OS_is_greater_than_min()
        {
            Mocker.GetMock<IEnvironmentProxy>()
                  .SetupGet(s => s.WindowsVersion)
                  .Returns(WindowsVersion.Eight);

            Subject.IsRequirementSatisfied(new Installer
                                           {
                                               MinWindowsVersion = WindowsVersion.Seven
                                           }).Success.Should().BeTrue();
        }

        [Test]
        public void should_be_false_when_OS_is_less_than_min()
        {
            Mocker.GetMock<IEnvironmentProxy>()
                  .SetupGet(s => s.WindowsVersion)
                  .Returns(WindowsVersion.Seven);

            Subject.IsRequirementSatisfied(new Installer
                                           {
                                               MinWindowsVersion = WindowsVersion.Eight
                                           }).Success.Should().BeFalse();
        }
    }
}
