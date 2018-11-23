using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using AppGet.Github.Releases;
using AppGet.Installers;
using AppGet.Manifest;
using NLog;
using Installer = AppGet.Manifest.Installer;

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
        private readonly Logger _logger;
        private Task<AppGetRelease> _releaseTask;
        private Lazy<IInstallService> _installService;

        public AppGetUpdateService(IReleaseClient releaseClient, Lazy<IInstallService> installService, Logger logger)
        {
            _releaseClient = releaseClient;
            _installService = installService;
            _logger = logger;
        }

        public void Start()
        {
            _releaseTask = _releaseClient.GetLatest();
        }

        public async Task Commit()
        {
            try
            {
                var latest = await _releaseTask;

                var current = Assembly.GetEntryAssembly().GetName().Version;

                _logger.Trace($"AppGet update status:  Current: {current}    Latest: {latest.Version}");

                if (latest.Version <= current) return;

                _logger.Info("There is an update available for AppGet client. Applying update...");

                var manifest = new PackageManifest
                {
                    Id = "AppGet",
                    InstallMethod = InstallMethodTypes.Inno,
                    Name = "AppGet",
                    Version = latest.Version.ToString(),
                    Home = "https://appget.net/",
                    Installers = new List<Installer>
                    {
                        new Installer
                        {
                            Location = latest.Url
                        }
                    }
                };

                await _installService.Value.Install(manifest, InstallInteractivityLevel.Interactive);
            }
            catch (Exception e)
            {
                _logger.Fatal(e, "AppGet update failed.");
            }
        }
    }
}