using System.Security.AccessControl;
using System.Security.Principal;
using AppGet.FileSystem;
using AppGet.HostSystem;
using AppGet.Infrastructure.Events;
using JetBrains.Annotations;

namespace AppGet.AppData
{
    [UsedImplicitly]
    public class AppDataService : IHandle<ApplicationStartingEvent>
    {
        private readonly IPathResolver _pathResolver;
        private readonly IFileSystem _fileSystem;

        public AppDataService(IPathResolver pathResolver, IFileSystem fileSystem)
        {
            _pathResolver = pathResolver;
            _fileSystem = fileSystem;
        }

        public void Handle(ApplicationStartingEvent message)
        {
            var path = _pathResolver.AppDataDirectory;

            _fileSystem.CreateDirectory(path);
            _fileSystem.SetPermissions(path, WellKnownSidType.WorldSid, FileSystemRights.FullControl, AccessControlType.Allow);
        }
    }
}