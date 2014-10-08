using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests
{
    [TestFixture]
    public class ArgumentsFixture
    {
        [TestCase("install /t /c=12 /awd")]
        [TestCase("INSTALL /t /c=12 /awd")]
        public void should_parse_command_name(string args)
        {
            var arguments = new Arguments(args);
            arguments.Command.Should().Be("install");
        }

        [TestCase("install /t /c=12 /awd")]
        [TestCase("INSTALL -t -c=12 -awd")]
        [TestCase("INSTALL /t /awd")]
        [TestCase("INSTALL -t -awd")]
        [TestCase("INSTALL -t /awd")]
        [TestCase("INSTALL /t /awd /url:http://test")]
        [TestCase("INSTALL -t text -awd -url:http://test")]
        public void should_parse_flags_name(string args)
        {
            var arguments = new Arguments(args);
            arguments.Flags.Should().HaveCount(2);
            arguments.Flags.Should().Contain("t");
            arguments.Flags.Should().Contain("awd");
        }

        [TestCase("install /t /c=12 /awd", "c", "12")]
        [TestCase("INSTALL -t -c=12 -awd", "c", "12")]
        [TestCase("install /url:http://test", "url", "http://test")]
        [TestCase("install -url:http://test", "url", "http://test")]
        [TestCase("install /url=http://test", "url", "http://test")]
        [TestCase("install -url=http://test", "url", "http://test")]
        [TestCase("install -url:\"C:\\Path With Spaces\\\"", "url", "C:\\Path With Spaces\\")]
        public void should_parse_params(string args, string expectedParam, string expectedValue)
        {
            var arguments = new Arguments(args);
            arguments.Params.Should().HaveCount(1);
            arguments.Params.First().Key.Should().Be(expectedParam);
            arguments.Params.First().Value.Should().Be(expectedValue);
        }


     
    }
}