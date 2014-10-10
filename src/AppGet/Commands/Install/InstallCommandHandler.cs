using System;
using AppGet.Options;
using AppGet.PackageProvider;
using NLog;

namespace AppGet.Commands.Install
{
    public class InstallCommandHandler : ICommandHandler
    {
        private readonly IPackageProvider _packageProvider;
        private readonly Logger _logger;

        public InstallCommandHandler(IPackageProvider packageProvider, Logger logger)
        {
            _packageProvider = packageProvider;
            _logger = logger;
        }

        public bool CanExecute(CommandOptions commandOptions)
        {
            return commandOptions is InstallOptions;
        }

        public void Execute(CommandOptions commandOptions)
        {
            _logger.Info("Getting flightplan for " + commandOptions.PackageName);
            _packageProvider.GetFlightPlan(commandOptions.PackageName);
        }
    }
}
