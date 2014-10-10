using AppGet.Commands;
using AppGet.Commands.Install;
using AppGet.Commands.ShowFlightPlan;
using AppGet.Download;
using NLog;
using TinyIoC;

namespace AppGet.Composition
{
    public static class ContainerBuilder
    {
        public static TinyIoCContainer Build()
        {
            var container = new TinyIoCContainer();

            Logger logger = LogManager.GetLogger("appget");

            container.AutoRegister(new[] { typeof(ContainerBuilder).Assembly });
            container.Register(logger);

            RegisterDownloadClients(container);
            RegisterCommandHandlersClients(container);

            return container;
        }

        private static void RegisterCommandHandlersClients(TinyIoCContainer container)
        {
            container.RegisterMultiple<ICommandHandler>(new[]
            {
                typeof(ShowFlightPlanCommandHandler),
                typeof(InstallCommandHandler)
            });
        }

        private static void RegisterDownloadClients(TinyIoCContainer container)
        {
            container.RegisterMultiple<IDownloadClient>(new[]
            {
                typeof(HttpDownloadClient)
            });
        }
    }
}