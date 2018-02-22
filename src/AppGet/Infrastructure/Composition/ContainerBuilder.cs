using AppGet.Commands;
using AppGet.Commands.CreateManifest;
using AppGet.Commands.Install;
using AppGet.Commands.List;
using AppGet.Commands.Search;
using AppGet.Commands.Uninstall;
using AppGet.Commands.ViewManifest;
using AppGet.Commands.WindowsInstallerSearch;
using AppGet.CreatePackage.InstallerPopulators;
using AppGet.CreatePackage.ManifestPopulators;
using AppGet.Crypto.Hash;
using AppGet.Crypto.Hash.Algorithms;
using AppGet.FileTransfer;
using AppGet.FileTransfer.Protocols;
using AppGet.Installers;
using AppGet.Installers.Exe;
using AppGet.Installers.Inno;
using AppGet.Installers.InstallShield;
using AppGet.Installers.Msi;
using AppGet.Installers.Nsis;
using AppGet.Installers.Squirrel;
using AppGet.Installers.Zip;
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

            container.AutoRegister(new[] { typeof(ContainerBuilder).Assembly });
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
                typeof(ListCommandHandler),
                typeof(InstallCommandHandler),
                typeof(WindowsInstallerSearchCommandHandler),
                typeof(UninstallCommandHandler),
                typeof(CreateManifestCommandHandler)
            });

            container.RegisterMultiple<IInstallerWhisperer>(new[]
            {
                typeof(InnoWhisperer),
                typeof(InstallShieldWhisperer),
                typeof(MsiWhisperer),
                typeof(NsisWhisperer),
                typeof(SquirrelWhisperer),
                typeof(ZipWhisperer),
                typeof(ExeWhisperer)

            });

            container.RegisterMultiple<ICheckSum>(new[]
            {
                typeof(Sha256Hash),
                typeof(Sha1Hash),
                typeof(Md5Hash),
            });

            container.RegisterMultiple<IPopulateManifest>(new[]
            {
                typeof(PopulateProductName),
                typeof(PopulatePackageId),
                typeof(PopulateVersion),
                typeof(PopulateVersionTag),
                typeof(PopulateProductUrl),
                typeof(PopulateLicense),
                typeof(PopulateInstallMethod)
            });


            container.RegisterMultiple<IPopulateInstaller>(new[]
            {
                typeof(ArchitecturePopulater),
                typeof(MinWindowsVersionPopulater)
            });

            container.RegisterMultiple<IDetectInstallMethod>(new[]
            {
                typeof(MsiDetector),
                typeof(SquirrelDetector),
                typeof(NsisDetector),
                typeof(InnoDetector)
            });


            container.RegisterMultiple<IFileTransferClient>(new[]
            {
                typeof(HttpFileTransferClient),
                typeof(WindowsPathFileTransferClient)
            });


            container.RegisterMultiple<IPackageRepository>(new[]
            {
                typeof(LocalPackageRepository),
                typeof(OfficialPackageRepository)
            });

            container.Register<IPackageRepository, AggregateRepository>();
        }

    }
}