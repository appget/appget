using System.Collections.Generic;
using System.Linq;
using AppGet.Commands;
using AppGet.CreatePackage.Installer;
using AppGet.CreatePackage.Root;
using AppGet.Crypto.Hash;
using AppGet.FileTransfer;
using AppGet.Infrastructure.Composition;
using AppGet.Installers;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Infrastructure.Composition
{
    [TestFixture]
    public class ContainerBuilderFixture
    {
        [Test]
        public void check_multi_registration()
        {
            var container = ContainerBuilder.Build();

            var allTypes = typeof(ContainerBuilder).Assembly.DefinedTypes.Where(c => !c.IsAbstract).ToList();

            void Assert<T>()
            {
                var commandHandlesr = allTypes.Where(c => c.ImplementedInterfaces.Any(i => i == typeof(T))).Select(x => x.Name).OrderBy(o => o).ToList();
                var registered = container.Resolve<IEnumerable<T>>().Select(e => e.GetType().Name).OrderBy(o => o).ToList();
                commandHandlesr.Should().Equal(registered);
            }

            Assert<ICommandHandler>();
            Assert<IInstallerWhisperer>();
            Assert<ICheckSum>();
            Assert<IExtractToManifestRoot>();
            Assert<IManifestPrompt>();
            Assert<IExtractToInstaller>();
            Assert<IInstallerPrompt>();
            Assert<IDetectInstallMethod>();
            Assert<IFileTransferClient>();
//            Assert<IPackageRepository>();
        }
    }
}