using AppGet.HostSystem;
using AppGet.Manifest.Hash;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.HostSystem
{
    public class MachineIdFixture : TestBase<MachineId>
    {
        [Test]
        public void get_machine_guid()
        {
            Mocker.SetInstance<ICalculateHash>(new Sha256());

            var guid = Subject.MachineKey.Value;

            guid.Should().NotBeEmpty();
            guid.Should().NotContain("=");
        }
    }
}