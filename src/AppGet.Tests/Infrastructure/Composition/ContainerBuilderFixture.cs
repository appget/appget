using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppGet.Commands;
using AppGet.CreatePackage.Installer;
using AppGet.CreatePackage.Root;
using AppGet.FileTransfer;
using AppGet.Infrastructure.Composition;
using AppGet.Installers.InstallerWhisperer;
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
            var container = ContainerBuilder.Container;

            var allTypes = Assembly.Load("AppGet").DefinedTypes.Where(c => !c.IsAbstract).ToList();

            void Assert<T>()
            {
                var super = typeof(T);
                var commandHandler = allTypes.Where(c => c.IsSubclassOf(super) || c.ImplementedInterfaces.Any(d => d == super)).Select(x => x.Name).OrderBy(o => o).ToList();
                var registered = container.Resolve<IEnumerable<T>>().Select(e => e.GetType().Name).OrderBy(o => o).ToList();
                commandHandler.Should().Equal(registered);
            }

            Assert<ICommandHandler>();
            Assert<InstallerBase>();
            Assert<IExtractToManifestRoot>();
            Assert<IManifestPrompt>();
            Assert<IInstallerPrompt>();
            Assert<IFileTransferClient>();
        }


        [Test]
        public void installer_whisperer_check()
        {
            var container = ContainerBuilder.Container;

            var whisperers = container.Resolve<IEnumerable<InstallerBase>>();

            whisperers.Should().OnlyHaveUniqueItems(c => c.InstallMethod);
        }
    }
}