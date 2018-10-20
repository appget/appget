using System.Threading.Tasks;
using AppGet.Commands;
using AppGet.Infrastructure.Composition;
using AppGet.Infrastructure.Logging;
using AppGet.Update;
using DryIoc;
using NLog;

namespace AppGet
{
    public static class Application
    {
        public static async Task Execute(string[] args)
        {
            LogConfigurator.EnableConsoleTarget(LogConfigurator.FriendlyLayout, LogLevel.Info);
            LogConfigurator.EnableSentryTarget("https://aa5e806801bc4d4f99a6112160128dbe@sentry.appget.net/7");
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