using System.Linq;
using System.Threading.Tasks;
using AppGet.CommandLine;
using AppGet.Infrastructure.Composition;
using AppGet.Update;
using Colorful;
using NLog;

namespace AppGet.Commands.List
{
    [Handles(typeof(ListOptions))]
    public class ListCommandHandler : ICommandHandler
    {
        private readonly UpdateService _updateService;
        private readonly Logger _logger;

        public ListCommandHandler(UpdateService updateService, Logger logger)
        {
            _updateService = updateService;
            _logger = logger;
        }


        public async Task Execute(AppGetOption commandOptions)
        {
            var matches = await _updateService.GetUpdates();

            var sorted = matches.OrderByDescending(c => c.Status);

            sorted.ShowTable();

            Console.WriteLine();
            _logger.Info($"Total Applications: {matches.Count:n0}   Updates Available: {matches.Count(c => c.Status == UpdateStatus.Available):n0}");
            Console.WriteLine();

            if (matches.Any(c => c.Status == UpdateStatus.Available))
            {
                Console.WriteLine("Run 'appget update-all' to apply all updates.");
            }
        }
    }
}