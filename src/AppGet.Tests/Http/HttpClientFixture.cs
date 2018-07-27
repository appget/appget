using System;
using AppGet.HostSystem;
using AppGet.Http;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Http
{
    [TestFixture]
    public class HttpClientFixture : TestBase<HttpClient>
    {
        [Test]
        public void should_send_gzip_headers()
        {
            Mocker.SetInstance<IUserAgentBuilder>(new UserAgentBuilder(new EnvInfo()));
            var res = Subject.Get(new Uri("https://api.appget.net/packages?q=vlc"));

            res.EnsureSuccessStatusCode();
            res.RequestMessage.Headers.AcceptEncoding.ToString().Should().Contain("gzip");
        }
    }
}