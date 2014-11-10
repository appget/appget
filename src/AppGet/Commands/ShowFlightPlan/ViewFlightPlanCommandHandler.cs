using System;
using System.Linq;
using AppGet.Commands.Install;
using AppGet.FileTransfer;
using AppGet.FlightPlans;
using AppGet.HostSystem;
using AppGet.InstalledPackages;
using AppGet.Installers;
using AppGet.Options;
using AppGet.PackageRepository;
using AppGet.Packages;
using NLog;

namespace AppGet.Commands.ShowFlightPlan
{
    public class ViewFlightPlanCommandHandler : ICommandHandler
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPathResolver _pathResolver;
        private readonly IFileTransferService _fileTransferService;
        private readonly IFlightPlanService _flightPlanService;
        private readonly IInstallService _installService;
        private readonly IFindInstaller _findInstaller;
        private readonly IInventoryManager _inventoryManager;
        private readonly Logger _logger;

        public ViewFlightPlanCommandHandler(IPackageRepository packageRepository, IPathResolver pathResolver, IFileTransferService fileTransferService,
            IFlightPlanService flightPlanService, IInstallService installService, IFindInstaller findInstaller, IInventoryManager inventoryManager, Logger logger)
        {
            _packageRepository = packageRepository;
            _pathResolver = pathResolver;
            _fileTransferService = fileTransferService;
            _flightPlanService = flightPlanService;
            _installService = installService;
            _findInstaller = findInstaller;
            _inventoryManager = inventoryManager;
            _logger = logger;
        }

        public bool CanExecute(AppGetOption packageCommandOptions)
        {
            return packageCommandOptions is ViewFlightPlanOptions;
        }

        public void Execute(AppGetOption packageCommandOptions)
        {

            var viewOptions = (ViewFlightPlanOptions)packageCommandOptions;

            var package = _packageRepository.GetLatest(viewOptions.PackageId);
            if (package == null)
            {
                throw new PackageNotFoundException(viewOptions.PackageId);
            }

            var flightPlan = _flightPlanService.ReadFlightPlan(package);
            Console.WriteLine("===============================================");
            Console.WriteLine();
            Console.WriteLine(flightPlan);
            Console.WriteLine();
            Console.WriteLine("===============================================");


        }
    }
}