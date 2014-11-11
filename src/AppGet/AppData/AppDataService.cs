using System.Security.AccessControl;
using System.Security.Principal;
using AppGet.FileSystem;
using AppGet.HostSystem;

namespace AppGet.AppData
{
    public interface IAppDataService
    {
        void EnsureAppDataDirectoryExists();
    }

    public class AppDataService : IAppDataService
    {
        private readonly IPathResolver _pathResolver;
        private readonly IFileSystem _fileSystem;

        public AppDataService(IPathResolver pathResolver, IFileSystem fileSystem)
        {
            _pathResolver = pathResolver;
            _fileSystem = fileSystem;
        }

        public void EnsureAppDataDirectoryExists()
        {
            var path = _pathResolver.AppDataDirectory;

            _fileSystem.CreateDirectory(path);
            _fileSystem.SetPermissions(path, WellKnownSidType.WorldSid, FileSystemRights.FullControl, AccessControlType.Allow);
        }
    }
}
