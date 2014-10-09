using AppGet.Environment;
using AppGet.FlightPlans;
using AppGet.Requirements.Specifications;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Requirements
{
    [TestFixture]
    public class OsArchitectureSpecificationFixture : TestBase<OsArchitectureSpecification>
    {
        [Test]
        public void should_be_true_when_type_is_x86()
        {
            Mocker.GetMock<IEnvironmentProxy>()
                  .SetupGet(s => s.Is64BitOperatingSystem)
                  .Returns(true);

            Subject.IsRequirementSatisfied(new PackageSource
                                           {
                                               Architecture = ArchitectureType.x86
                                           }).Should().BeTrue();

        }

        [Test]
        public void should_be_true_when_type_is_x64_on_x64_os()
        {
            Mocker.GetMock<IEnvironmentProxy>()
                  .SetupGet(s => s.Is64BitOperatingSystem)
                  .Returns(true);

            Subject.IsRequirementSatisfied(new PackageSource
                                           {
                                               Architecture = ArchitectureType.x64
                                           }).Should().BeTrue();
        }

        [Test]
        public void should_be_false_when_x64_on_x86_os()
        {
            Mocker.GetMock<IEnvironmentProxy>()
                  .SetupGet(s => s.Is64BitOperatingSystem)
                  .Returns(false);

            Subject.IsRequirementSatisfied(new PackageSource
                                           {
                                               Architecture = ArchitectureType.x64
                                           }).Should().BeFalse();
        }

        [Test]
        public void should_be_false_when_itanium()
        {
            Subject.IsRequirementSatisfied(new PackageSource
            {
                Architecture = ArchitectureType.Itanium
            }).Should().BeFalse();
        }

        [Test]
        public void should_be_false_when_arm()
        {
            Subject.IsRequirementSatisfied(new PackageSource
            {
                Architecture = ArchitectureType.ARM
            }).Should().BeFalse();
        }
    }
}
