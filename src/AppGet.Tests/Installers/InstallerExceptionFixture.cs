using System.Diagnostics;
using AppGet.Installers;
using AppGet.Manifest;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AppGet.Tests.Installers
{
    [TestFixture]
    public class InstallerExceptionFixture
    {
        [Test]
        public void should_have_exit_code_reason_in_the_message()
        {
            var man = new PackageManifest
            {
                Id = "Test",
                Version = "1.2.3"
            };
            var ex = new InstallerException(1030, man, new ExistReason(ExitCodeTypes.RestartRequired, null, true), null);


            ex.Message.Should().Contain("restart");
        }

    }
}