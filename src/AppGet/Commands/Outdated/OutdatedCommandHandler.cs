using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using AppGet.CommandLine;
using AppGet.Update;
using Console = Colorful.Console;

namespace AppGet.Commands.Outdated
{
    public class OutdatedCommandHandler : ICommandHandler
    {
        private readonly UpdateService _updateService;

        public OutdatedCommandHandler(UpdateService updateService)
        {
            _updateService = updateService;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is OutdatedOptions;
        }

        public async Task Execute(AppGetOption commandOptions)
        {
            var updates = await _updateService.GetUpdates();

            Console.WriteLine("Available Updates:");
            updates.Where(c => c.Status == UpdateStatus.Available).ShowTable();


            if (commandOptions.Verbose)
            {
                Console.WriteLine("");
                Console.WriteLine("Latest Version Already Installed:", Color.Green);

                var upToDate = updates.Where(c => c.Status == UpdateStatus.UpToDate);

                upToDate.ShowTable();
            }
        }
    }
}