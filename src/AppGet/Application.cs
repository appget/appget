using System.Threading.Tasks;
using AppGet.Commands;
using AppGet.Infrastructure.Composition;
using AppGet.Update;

namespace AppGet
{
    public static class Application
    {
        public static async Task Execute(string[] args)
        {
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