using System;
using AppGet.Extensions;
using NUnit.Framework;

namespace AppGet.Tests.Extensions
{
    public class UriExtensionsFixture
    {
        [TestFixture]
        public class StringExtensionsFixture
        {
            [TestCase("http://google.com/", ExpectedResult = "https://google.com/")]
            [TestCase("https://google.com/", ExpectedResult = "https://google.com/")]
            public string Alphanumeric(string url)
            {
                return new Uri(url).ToHttps().ToString();
            }
        }
    }
}