using System;
using System.Linq;
using AppGet.Commands;
using AppGet.CreatePackage.Installer;
using AppGet.CreatePackage.Root;
using AppGet.FileTransfer;
using AppGet.Infrastructure.Composition;
using AppGet.Installers.InstallerWhisperer;
using AppGet.Installers.UninstallerWhisperer;
using DryIoc;
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

            void Assert<T>()
            {
                var commandHandler = ContainerBuilder.AssemblyTypes.Where(c => c.ImplementsServiceType<T>()).ToList();
                var registered = container.ResolveMany<T>().Select(e => e.GetType()).ToList();
                commandHandler.Should().BeEquivalentTo(registered);
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

            var whisperersFac = container.Resolve<Func<InstallerBase[]>>();
            var whisperers = whisperersFac();
            whisperers.Should().OnlyHaveUniqueItems(c => c.InstallMethod);
        }

        [Test]
        public void uninstaller_whisperer_check()
        {
            var container = ContainerBuilder.Container;

            var whisperersFac = container.Resolve<Func<UninstallerBase[]>>();
            var whisperers = whisperersFac();
            whisperers.Should().OnlyHaveUniqueItems(c => c.InstallMethod);
        }

        [Test]
        public void installers_should_be_transient()
        {
            var container = ContainerBuilder.Container;

            var whisperersFac = container.Resolve<Func<InstallerBase[]>>();
            var whisperers1 = whisperersFac();
            var whisperers2 = whisperersFac();


            whisperers2.Should().NotEqual(whisperers1);
        }

        
        [Test]
        public void uninstallers_should_be_transient()
        {
            var container = ContainerBuilder.Container;

            var whisperersFac = container.Resolve<Func<UninstallerBase[]>>();
            var whisperers1 = whisperersFac();
            var whisperers2 = whisperersFac();


            whisperers2.Should().NotEqual(whisperers1);
        }


        [Test]
        public void check_commands()
        {
            var container = ContainerBuilder.Container;

            var handlers = container.ResolveMany<ICommandHandler>();

            handlers.Should().NotBeEmpty();
        }
    }
}