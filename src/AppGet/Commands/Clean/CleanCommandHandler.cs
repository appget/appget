using System.Threading.Tasks;
using AppGet.FileSystem;
using AppGet.HostSystem;
using AppGet.Infrastructure.Composition;
using NLog;

namespace AppGet.Commands.Clean
{
    [Handles(typeof(CleanOptions))]
    public class CleanCommandHandler : ICommandHandler
    {
        private readonly IFileSystem _fileSystem;
        private readonly IPathResolver _pathResolver;
        private readonly Logger _logger;

        public CleanCommandHandler(IFileSystem fileSystem, IPathResolver pathResolver, Logger logger)
        {
            _fileSystem = fileSystem;
            _pathResolver = pathResolver;
            _logger = logger;
        }

        public async Task Execute(AppGetOption commandOptions)
        {
            _logger.Info("Clearing Installer Cache.");
            var deleted = _fileSystem.ClearDirectory(_pathResolver.InstallerCacheFolder, true);

            _logger.Info("Clearing Temp folder.");
            deleted += _fileSystem.ClearDirectory(_pathResolver.TempFolder, true);
            _logger.Info("Cleared cache folders. Total number of files deleted: {0:n0}", deleted);
        }
    }
}