//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using AppGet.Commands;
//using AppGet.Commands.CreateManifest;
//using AppGet.Commands.Install;
//using AppGet.Commands.Outdated;
//using AppGet.Commands.Search;
//using AppGet.Commands.Uninstall;
//using AppGet.Commands.Update;
//using AppGet.Commands.ViewManifest;
//using AppGet.CreatePackage.Installer;
//using AppGet.CreatePackage.Installer.Prompts;
//using AppGet.CreatePackage.Root;
//using AppGet.CreatePackage.Root.Prompts;
//using AppGet.FileTransfer;
//using AppGet.FileTransfer.Protocols;
//using AppGet.Infrastructure.Events;
//using AppGet.Infrastructure.Logging;
//using AppGet.Installers.InstallerWhisperer;
//using AppGet.Installers.UninstallerWhisperer;
//using AppGet.Manifest;
//using AppGet.PackageRepository;
//using NLog;
//using InnoWhisperer = AppGet.Installers.InstallerWhisperer.InnoWhisperer;
//using NsisWhisperer = AppGet.Installers.InstallerWhisperer.NsisWhisperer;
//using SquirrelWhisperer = AppGet.Installers.InstallerWhisperer.SquirrelWhisperer;
//
//namespace AppGet.Infrastructure.Composition
//{
//    public static class ContainerBuilder
//    {
//        public static readonly List<Type> AssemblyTypes;
//
//        static ContainerBuilder()
//        {
//            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "appget.*", SearchOption.TopDirectoryOnly).Where(c => c.EndsWith(".exe") || c.EndsWith(".dll"));
//
//
//            var ass = files.Select(Assembly.LoadFile);
//
//            AssemblyTypes = ass.SelectMany(c => c.ExportedTypes.Where(t => !t.IsAbstract && !t.IsInterface && !t.IsEnum && t.Namespace != null && t.Namespace.StartsWith("AppGet."))).ToList();
//
//            LogConfigurator.ConfigureLogger();
//            Container = Build();
//
//
//            Container.Resolve<EventHub>().Publish(new ApplicationStartingEvent(Container));
//
//        }
//
//        public static TinyIoCContainer Container { get; private set; }
//
//        private static TinyIoCContainer Build()
//        {
//            var container = new TinyIoCContainer();
//
//
//            //            container.Register<ITinyMessengerHub>(new TinyMessengerHub(new DefaultSubscriberErrorHandler())).AsSingleton();
//
//            var logger = LogManager.GetLogger("appget");
//
//
//            foreach (var assemblyType in AssemblyTypes)
//            {
//                container.Register(assemblyType).AsSingleton();
//            }
//
////            container.AutoRegister(new[]
////            {
////                typeof(ContainerBuilder).Assembly,
////                typeof(PackageManifest).Assembly
////            }, DuplicateImplementationActions.RegisterSingle, t =>
////            {
////                if (t.IsAssignableFrom(typeof(AppGetOption)))
////                {
////                    return false;
////                }
////                return true;
////            });
//            container.Register(logger);
//
//            RegisterLists(container);
//
//            container.Unregister<TinyMessengerHub>();
//
//            return container;
//        }
//
//
//        private static void RegisterMultiple<T>(this TinyIoCContainer container)
//        {
//            var implementations = AssemblyTypes.Where(c => typeof(T).IsAssignableFrom(c));
//            container.RegisterMultiple<T>(implementations);
//        }
//
//        private static void RegisterLists(TinyIoCContainer container)
//        {
//            container.Register<ICommandHandler, ViewManifestCommandHandler>(typeof(ViewManifestOptions).FullName);
//            container.Register<ICommandHandler, SearchCommandHandler>(typeof(SearchOptions).FullName);
//            container.Register<ICommandHandler, InstallCommandHandler>(typeof(InstallOptions).FullName);
//            container.Register<ICommandHandler, UninstallCommandHandler>(typeof(UninstallOptions).FullName);
//            container.Register<ICommandHandler, CreateManifestCommandHandler>(typeof(CreateManifestOptions).FullName);
//            container.Register<ICommandHandler, UpdateCommandHandler>(typeof(UpdateOptions).FullName);
//            container.Register<ICommandHandler, OutdatedCommandHandler>(typeof(OutdatedOptions).FullName);
//
//
//            //            container.RegisterMultiple<IHandle>();
//
//            container.RegisterMultiple<IHandle<ApplicationStartingEvent>>();
//            container.RegisterMultiple<IHandle<ManifestLoadedEvent>>();
//            container.RegisterMultiple<IHandle<FileTransferCompletedEvent>>();
//            container.RegisterMultiple<IHandle<FileTransferStartedEvent>>();
//
//            container.RegisterMultiple<IManifestPrompt>(new[]
//            {
//                typeof(ProductNamePrompt),
//                typeof(PackageIdPrompt),
//                typeof(VersionPrompt),
//                typeof(HomePagePrompt),
//                typeof(LicensePrompt),
//                typeof(InstallMethodPrompt),
//                typeof(TagPrompt),
//            });
//
//            container.RegisterMultiple<IInstallerPrompt>(new[]
//            {
//                typeof(ArchitecturePrompt),
//                typeof(MinWindowsVersionPrompt)
//            });
//
//            container.RegisterMultiple<InstallerBase>(new[]
//            {
//                typeof(CustomWhisperer),
//                typeof(InnoWhisperer),
//                typeof(InstallBuilderWhisperer),
//                typeof(InstallShieldWhisperer),
//                typeof(MsiWhisperer),
//                typeof(WixWhisperer),
//                typeof(NsisWhisperer),
//                typeof(SquirrelWhisperer),
//                typeof(AdvancedInstallerWhisperer),
//                typeof(SetupFactoryWhisperer),
//            });
//
//            container.RegisterMultiple<UninstallerBase>(new[]
//            {
//                typeof(SquirrelUninstaller),
//                typeof(WixUninstaller),
//                typeof(MsiUninstaller),
//                typeof(NsisUninstaller),
//                typeof(InnoUninstaller),
//            });
//
//            container.RegisterMultiple<IFileTransferClient>(new[]
//            {
//                typeof(HttpFileTransferClient),
//            });
//
//            container.RegisterMultiple<IPackageRepository>(new[]
//            {
////                typeof(LocalPackageRepository),
//                typeof(OfficialPackageRepository)
//            });
//
//            container.Register<IPackageRepository, OfficialPackageRepository>();
//        }
//    }
//}