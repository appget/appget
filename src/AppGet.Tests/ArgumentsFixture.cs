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
        [TestCase("INSTALL -t -c=12 =awd")]
        [TestCase("INSTALL /t /awd")]
        [TestCase("INSTALL -t -awd")]
        [TestCase("INSTALL -t /awd")]
        [TestCase("INSTALL /t /awd /url:http://test")]
        [TestCase("INSTALL -t -awd -url:http://test")]
        public void should_parse_flags_name(string args)
        {
            var arguments = new Arguments(args);
            arguments.Flags.Should().HaveCount(2);
            arguments.Flags.Should().Contain("t");
            arguments.Flags.Should().Contain("awd");
        }

    }
}