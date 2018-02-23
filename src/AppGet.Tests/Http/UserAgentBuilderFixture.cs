using AppGet.HostSystem;
using AppGet.Http;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Http
{
    [TestFixture]
    public class UserAgentBuilderFixture : TestBase<UserAgentBuilder>
    {
        [Test]
        public void should_generate_user_agents()
        {
            Mocker.SetInstance<IOsInfo>(new OsInfo(logger));

            Subject.GetUserAgent(false).Should().NotBeNullOrWhiteSpace();
            Subject.GetUserAgent(true).Should().NotBeNullOrWhiteSpace();
        }
    }
}