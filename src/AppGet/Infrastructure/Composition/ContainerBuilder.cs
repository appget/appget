using AppGet.Commands;
using AppGet.Commands.Install;
using AppGet.Commands.List;
using AppGet.Commands.ShowFlightPlan;
using AppGet.Commands.Uninstall;
using AppGet.FileTransfer;
using AppGet.FileTransfer.Protocols;
using AppGet.Installers;
using AppGet.Installers.Msi;
using AppGet.Installers.Zip;
using NLog;
using TinyIoC;

namespace AppGet.Infrastructure.Composition
{
    public static class ContainerBuilder
    {
        public static TinyIoCContainer Build()
        {
            var container = new TinyIoCContainer();

            Logger logger = LogManager.GetLogger("appget");

            container.AutoRegister(new[] { typeof(ContainerBuilder).Assembly });
            container.Register(logger);

            RegisterLists(container);

            return container;
        }

        private static void RegisterLists(TinyIoCContainer container)
        {
            container.RegisterMultiple<ICommandHandler>(new[]
            {
                typeof(ShowFlightPlanCommandHandler),
                typeof(ListCommandHandler),
                typeof(InstallCommandHandler),
                typeof(UninstallCommandHandler)
            });

            container.RegisterMultiple<IInstallerWhisperer>(new[]
            {
                typeof(MsiWhisperer),
                typeof(ZipWhisperer),
            });

            container.RegisterMultiple<IFileTransferClient>(new[]
            {
                typeof(HttpFileTransferClient)
            });
        }

    }
}