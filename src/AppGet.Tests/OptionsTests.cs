using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests
{
    [TestFixture]
    public class OptionsTests
    {
        [Test]
        public void should_parse_verb()
        {
            var option = Options.Parse(new[] { "showflightplan" });

            option.Should().BeOfType<ShowFlightPlanOptions>();
            option.Common.Should().NotBeNull();
        }
    }
}