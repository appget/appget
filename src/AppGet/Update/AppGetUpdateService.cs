using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AppGet.Commands.Install;
using AppGet.Installers;
using AppGet.Manifests;
using NLog;

namespace AppGet.Update
{
    public interface IAppGetUpdateService
    {
        void Start();
        Task Commit();
    }

    public class AppGetUpdateService : IAppGetUpdateService
    {
        private readonly IReleaseClient _releaseClient;
        private readonly IInstallService _installService;
        private readonly Logger _logger;
        private Task<List<AppGetRelease>> _releaseTask;

        public AppGetUpdateService(IReleaseClient releaseClient, IInstallService installService, Logger logger)
        {
            _releaseClient = releaseClient;
            _installService = installService;
            _logger = logger;
        }


        public void Start()
        {
            _releaseTask = _releaseClient.GetReleases();
        }


        public async Task Commit()
        {
            var releases = await _releaseTask;
            var latest = releases.OrderByDescending(c => c.Version).First();
            var current = Assembly.GetEntryAssembly().GetName().Version;

            _logger.Trace($"Update Status.  Current: {current}    Latest: {latest.Version}");

            if (latest.Version <= current) return;

            _logger.Info("There is an update avilable for AppGet client. Applying update...");

            var manifest = new PackageManifest
            {
                Id = "AppGet",
                InstallMethod = InstallMethodTypes.Inno,
                Name = "AppGet",
                Version = latest.Version.ToString(),
                ProductUrl = "https://appget.net",
                Installers = new List<Installer>
                {
                    new Installer
                    {
                        Location = latest.Url
                    }
                }
            };

            await _installService.Install(manifest, new InstallOptions { Force = true, Package = manifest.Id });
        }
    }
}
