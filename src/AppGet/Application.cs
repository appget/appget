using System.Threading.Tasks;
using AppGet.Commands;
using AppGet.Infrastructure.Composition;
using AppGet.Infrastructure.Logging;
using AppGet.Update;
using NLog;

namespace AppGet
{
    public static class Application
    {
        public static async Task Execute(string[] args)
        {
            LogConfigurator.EnableConsoleTarget(LogConfigurator.FriendlyLayout, LogLevel.Info);
            LogConfigurator.EnableSentryTarget("https://79eabeab1aa84c8db73ee2675c5bce7d@sentry.io/306545");
            LogConfigurator.EnableFileTarget(LogLevel.Trace);

            var container = ContainerBuilder.Container;

            IAppGetUpdateService updatedService = null;
            try
            {
                updatedService = container.Resolve<IAppGetUpdateService>();
                updatedService.Start();


                var commandExecutor = container.Resolve<ICommandExecutor>();
                await commandExecutor.ExecuteCommand(args);
            }
            finally
            {
                if (updatedService != null)
                {
                    await updatedService.Commit();
                }
            }
        }
    }
}