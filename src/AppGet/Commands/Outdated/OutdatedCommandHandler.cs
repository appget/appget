using System;
using AppGet.CommandLine;
using AppGet.Update;

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

        public void Execute(AppGetOption commandOptions)
        {
            var updates = _updateService.GetUpdates().Result;
            updates.ShowTable();
        }
    }
}