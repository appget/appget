using AppGet.Commands.Install;
using AppGet.Compression;
using AppGet.FileTransfer;
using AppGet.FlightPlans;
using AppGet.HostSystem;
using NLog;

namespace AppGet.Install
{
    public class ZipInstaller
    {
        private readonly Logger _logger;
        private readonly IFileTransferService _fileTransferService;
        private readonly IPathResolver _pathResolver;

        public ZipInstaller(Logger logger, CompressionService compressionService, IFileTransferService fileTransferService, IPathResolver pathResolver)
        {
            _logger = logger;
            _fileTransferService = fileTransferService;
            _pathResolver = pathResolver;
        }

        public void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions)
        {

        }
    }
}