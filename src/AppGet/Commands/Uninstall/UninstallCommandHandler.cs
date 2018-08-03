using System.Threading.Tasks;

namespace AppGet.Commands.Uninstall
{
    public class UninstallCommandHandler : ICommandHandler
    {
        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is UninstallOptions;
        }

        public async Task Execute(AppGetOption commandOptions)
        {
        }
    }
}