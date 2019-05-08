using AppGet.Commands;
using AppGet.Commands.Install;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Commands
{
    [TestFixture]
    public class CommandOptionExtensionsTests
    {
        [Test]
        public void should_create_dic_from_options()
        {
            var installCommand = new InstallOptions()
            {
                Interactive = true,
                Package = "vlc",
            };

            var dic = installCommand.ToDictionary();

            dic["cmd_package"].Should().Be(installCommand.Package);
            dic["cmd_force"].Should().Be("False");
            dic["cmd_interactivity"].Should().Be("interactive");
            dic["cmd_force"].Should().Be("false");
            dic["cmd_tag"].Should().Be(null);
            dic["cmd_commandname"].Should().Be("install");
        }
    }
}