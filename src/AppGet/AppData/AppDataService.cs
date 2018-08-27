using System.Security.AccessControl;
using System.Security.Principal;
using AppGet.FileSystem;
using AppGet.HostSystem;
using AppGet.Infrastructure.Eventing;
using AppGet.Infrastructure.Eventing.Events;
using JetBrains.Annotations;

namespace AppGet.AppData
{
    [UsedImplicitly]
    public class AppDataService : IHandle<ApplicationStartedEvent>
    {
        private readonly IPathResolver _pathResolver;
        private readonly IFileSystem _fileSystem;

        public AppDataService(IPathResolver pathResolver, IFileSystem fileSystem)
        {
            _pathResolver = pathResolver;
            _fileSystem = fileSystem;
        }

        public void Handle(ApplicationStartedEvent @event)
        {
            var path = _pathResolver.AppDataDirectory;

            _fileSystem.CreateDirectory(path);
            _fileSystem.SetPermissions(path, WellKnownSidType.WorldSid, FileSystemRights.FullControl, AccessControlType.Allow);
        }
    }
}