using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using AppGet.CommandLine;
using AppGet.Infrastructure.Composition;
using AppGet.Update;
using Colorful;
using NLog;

namespace AppGet.Commands.Outdated
{

    [Handles(typeof(OutdatedOptions))]
    public class OutdatedCommandHandler : ICommandHandler
    {
        private readonly UpdateService _updateService;
        private readonly Logger _logger;

        public OutdatedCommandHandler(UpdateService updateService, Logger logger)
        {
            _updateService = updateService;
            _logger = logger;
        }


        public async Task Execute(AppGetOption commandOptions)
        {
            var matches = await _updateService.GetUpdates();

            var updates = matches
                .Where(c => c.Status == UpdateStatus.Available)
                .OrderBy(c => c.PackageId)
                .ToList();

            if (updates.Any())
            {
                Console.WriteLine($"{updates.Count} Available Updates:");
                updates.ShowTable(false);

                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("No updates were found.");
            }


            if (commandOptions.Verbose)
            {
                Console.WriteLine("");
                Console.WriteLine("Latest Version Already Installed:", Color.Green);

                var upToDate = matches
                    .Where(c => c.Status == UpdateStatus.UpToDate)
                    .OrderBy(c => c.PackageId);

                upToDate.ShowTable(false);
            }

            Console.WriteLine();
            _logger.Info($"Total Applications: {matches.Count:n0}   Updates Available: {matches.Count(c => c.Status == UpdateStatus.Available):n0}");
            Console.WriteLine();

            if (updates.Any())
            {
                Console.WriteLine("Run 'appget update-all' to apply all updates.");
            }
        }
    }
}