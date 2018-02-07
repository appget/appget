
using AppGet.Extensions;
using NUnit.Framework;

namespace AppGet.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsFixture
    {
        [TestCase("a b c d", ExpectedResult = "abcd")]
        [TestCase("a b c d  ", ExpectedResult = "abcd")]
        [TestCase("a+b_c.d_&", ExpectedResult = "abcd")]
        [TestCase("ABC 12", ExpectedResult = "abc12")]
        [TestCase("d-a-s-h-e-s----", ExpectedResult = "dashes")]
        public string Alphanumeric(string source)
        {
            return source.ToAlphaNumeric();
        }
    }
}