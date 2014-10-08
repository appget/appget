using System;
using System.Linq;
using AppGet.Options;
using NLog;

namespace AppGet.Commands.ShowFlightPlan
{
    public class ShowFlightPlanCommandHandler : ICommandHandler
    {
        private readonly Logger _logger;

        public ShowFlightPlanCommandHandler(Logger logger)
        {
            _logger = logger;
        }

        public bool CanExecute(CommandOptions commandOptions)
        {
            return commandOptions is ShowFlightPlanOptions;
        }

        public void Execute(CommandOptions commandOptions)
        {
            var options = (ShowFlightPlanOptions)commandOptions;

            _logger.Info("bluup! " + options.PackageName);
        }
    }
}