using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AppGet.HostSystem;
using AppGet.Http;
using AppGet.Manifest;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using HttpClient = AppGet.Http.HttpClient;

namespace AppGet.Tests.Http
{
    [TestFixture]
    public class HttpClientFixture : TestBase<HttpClient>
    {
        [SetUp]
        public void Setup()
        {
            WithRealHttp();
        }

        [Test]
        public async Task should_send_gzip_headers()
        {
            Mocker.SetInstance<IUserAgentBuilder>(new UserAgentBuilder(new EnvInfo()));
            var res = await Subject.GetAsync(new Uri("https://nex.appget.net/packages?q=vlc"), TimeSpan.FromSeconds(10));

            res.EnsureSuccessStatusCode();
            res.RequestMessage.Headers.AcceptEncoding.ToString().Should().Contain("gzip");
        }

        [Test]
        public void should_throw_name_resolution_exception()
        {
            Mocker.SetInstance<IUserAgentBuilder>(new UserAgentBuilder(new EnvInfo()));
            var ex = Assert.ThrowsAsync<WebException>(async () => await Subject.GetAsync(new Uri("https://not-valid.appget.net"), TimeSpan.FromSeconds(10)));
            ex.Message.Should().Contain("The remote name could not be resolved");
        }

        [Test]
        public void should_throw_for_non_success()
        {
            Mocker.SetInstance<IUserAgentBuilder>(new UserAgentBuilder(new EnvInfo()));
            Assert.ThrowsAsync<HttpException>(async () => await Subject.GetAsync(new Uri("https://google.com/not-a-valid-path"), TimeSpan.FromSeconds(10)));
        }

        [Test]
        public async Task post_should_send_correct_encoding()
        {
            Mocker.SetInstance<IUserAgentBuilder>(new UserAgentBuilder(new EnvInfo()));

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri("https://httpbin.org/post"));
            request.Content = new JsonContent(new PackageManifest());

            var res = await Subject.SendAsync(request, TimeSpan.FromSeconds(10));
            res.EnsureSuccessStatusCode();

            var binResp = await res.Deserialize<HttpBinResponse>();

            binResp.Headers["Content-Type"].Should().Be("Application/Json; charset=utf-8");
        }
    }

   
}