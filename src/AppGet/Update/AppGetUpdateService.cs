using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AppGet.Commands.Install;
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
        private Task<List<AppGetRelease>> _releaseTask;
        private Lazy<IInstallService> _installService;

        public AppGetUpdateService(IReleaseClient releaseClient, Lazy<IInstallService> installService, Logger logger)
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
            try
            {
                var releases = await _releaseTask;
                var latest = releases.OrderByDescending(c => c.Version).First();
                var current = Assembly.GetEntryAssembly().GetName().Version;

                _logger.Trace($"AppGet update status:  Current: {current}    Latest: {latest.Version}");

                if (latest.Version <= current) return;

                _logger.Info("There is an update avilable for AppGet client. Applying update...");

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

                await _installService.Value.Install(manifest, new InstallOptions
                {
                    Force = true,
                    Package = manifest.Id
                });
            }
            catch (AggregateException e)
            {
                _logger.Fatal(e.Flatten().InnerExceptions.First(), "AppGet update failed");
            }
            catch (Exception e)
            {
                _logger.Fatal(e, "AppGet update failed");
            }
        }
    }
}