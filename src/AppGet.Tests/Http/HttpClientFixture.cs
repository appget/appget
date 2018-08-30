using System;
using System.Net;
using System.Threading.Tasks;
using AppGet.HostSystem;
using AppGet.Http;
using FluentAssertions;
using NUnit.Framework;
using HttpClient = AppGet.Http.HttpClient;

namespace AppGet.Tests.Http
{
    [TestFixture]
    public class HttpClientFixture : TestBase<HttpClient>
    {
        [Test]
        public async Task should_send_gzip_headers()
        {
            Mocker.SetInstance<IUserAgentBuilder>(new UserAgentBuilder(new EnvInfo()));
            var res = await Subject.GetAsync(new Uri("https://nex.appget.net/packages?q=vlc"));

            res.EnsureSuccessStatusCode();
            res.RequestMessage.Headers.AcceptEncoding.ToString().Should().Contain("gzip");
        }

        [Test]
        public void should_throw_name_resolution_exception()
        {
            Mocker.SetInstance<IUserAgentBuilder>(new UserAgentBuilder(new EnvInfo()));
            var ex = Assert.ThrowsAsync<WebException>(async () => await Subject.GetAsync(new Uri("https://not-valid.appget.net")));
            ex.Message.Should().Contain("The remote name could not be resolved");
        }


        [Test]
        public void should_throw_for_non_success()
        {
            Mocker.SetInstance<IUserAgentBuilder>(new UserAgentBuilder(new EnvInfo()));
            Assert.ThrowsAsync<HttpException>(async () => await Subject.GetAsync(new Uri("https://google.com/not-a-valid-path")));
        }
    }
}