using AppGet.Commands;
using AppGet.Commands.CreateManifest;
using AppGet.Commands.Install;
using AppGet.Commands.Outdated;
using AppGet.Commands.Search;
using AppGet.Commands.Uninstall;
using AppGet.Commands.ViewManifest;
using AppGet.CreatePackage.Installer;
using AppGet.CreatePackage.Installer.Prompts;
using AppGet.CreatePackage.Root;
using AppGet.CreatePackage.Root.Prompts;
using AppGet.FileTransfer;
using AppGet.FileTransfer.Protocols;
using AppGet.Installers;
using AppGet.Installers.AdvancedInstaller;
using AppGet.Installers.Custom;
using AppGet.Installers.Inno;
using AppGet.Installers.InstallBuilder;
using AppGet.Installers.InstallShield;
using AppGet.Installers.Msi;
using AppGet.Installers.Nsis;
using AppGet.Installers.SetupFactory;
using AppGet.Installers.Squirrel;
using AppGet.Installers.Wix;
using AppGet.Manifest;
using AppGet.PackageRepository;
using NLog;

namespace AppGet.Infrastructure.Composition
{
    public static class ContainerBuilder
    {
        public static TinyIoCContainer Build()
        {
            var container = new TinyIoCContainer();

            var logger = LogManager.GetLogger("appget");

            container.AutoRegister(new[]
            {
                typeof(ContainerBuilder).Assembly,
                typeof(PackageManifest).Assembly
            });
            container.Register(logger);

            RegisterLists(container);

            return container;
        }

        private static void RegisterLists(TinyIoCContainer container)
        {
            container.RegisterMultiple<ICommandHandler>(new[]
            {
                typeof(ViewManifestCommandHandler),
                typeof(SearchCommandHandler),
                typeof(InstallCommandHandler),
                typeof(UninstallCommandHandler),
                typeof(CreateManifestCommandHandler),
                typeof(OutdatedCommandHandler)
            });

            container.RegisterMultiple<IManifestPrompt>(new[]
            {
                typeof(ProductNamePrompt),
                typeof(PackageIdPrompt),
                typeof(VersionPrompt),
                typeof(HomePagePrompt),
                typeof(LicensePrompt),
                typeof(InstallMethodPrompt),
                typeof(TagPrompt),
            });

            container.RegisterMultiple<IInstallerPrompt>(new[]
            {
                typeof(ArchitecturePrompt),
                typeof(MinWindowsVersionPrompt)
            });

            container.RegisterMultiple<IInstallerWhisperer>(new[]
            {
                typeof(CustomWhisperer),
                typeof(InnoWhisperer),
                typeof(InstallBuilderWhisperer),
                typeof(InstallShieldWhisperer),
                typeof(MsiWhisperer),
                typeof(WixWhisperer),
                typeof(NsisWhisperer),
                typeof(SquirrelWhisperer),
                typeof(AdvancedInstallerWhisperer),
                typeof(SetupFactoryWhisperer),
            });

            container.RegisterMultiple<IFileTransferClient>(new[]
            {
                typeof(HttpFileTransferClient),
                typeof(WindowsPathFileTransferClient)
            });

            container.RegisterMultiple<IPackageRepository>(new[]
            {
//                typeof(LocalPackageRepository),
                typeof(OfficialPackageRepository)
            });

            container.Register<IPackageRepository, AggregateRepository>();
        }
    }
}