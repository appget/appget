using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using AppGet.CommandLine;
using AppGet.Infrastructure.Composition;
using AppGet.Update;
using Console = Colorful.Console;

namespace AppGet.Commands.Outdated
{

    [Handles(typeof(OutdatedOptions))]
    public class OutdatedCommandHandler : ICommandHandler
    {
        private readonly UpdateService _updateService;

        public OutdatedCommandHandler(UpdateService updateService)
        {
            _updateService = updateService;
        }


        public async Task Execute(AppGetOption commandOptions)
        {
            var matches = await _updateService.GetUpdates();

            var updates = matches.Where(c => c.Status == UpdateStatus.Available).ToList();
            if (updates.Any())
            {
                Console.WriteLine("{0} Available Updates:", updates.Count);
                updates.ShowTable();

                Console.WriteLine("");
                Console.WriteLine("Run 'appget update-all' to apply all updates.");

            }
            else
            {
                Console.WriteLine("No updates were found.");
            }


            if (commandOptions.Verbose)
            {
                Console.WriteLine("");
                Console.WriteLine("Latest Version Already Installed:", Color.Green);

                var upToDate = matches.Where(c => c.Status == UpdateStatus.UpToDate);

                upToDate.ShowTable();
            }
        }
    }
}