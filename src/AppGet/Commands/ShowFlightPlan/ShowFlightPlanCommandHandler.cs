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

        public bool CanExecute(AppGetOption packageCommandOptions)
        {
            return packageCommandOptions is ShowFlightPlanOptions;
        }

        public void Execute(AppGetOption packageCommandOptions)
        {
            var options = (ShowFlightPlanOptions)packageCommandOptions;

            _logger.Info("bluup! " + options.PackageName);
        }
    }
}