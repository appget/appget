using System;
using System.Threading.Tasks;
using System.Windows;
using AppGet.Commands;
using AppGet.Commands.Install;
using AppGet.FileTransfer;
using AppGet.Gui.Views.Shared;
using AppGet.Infrastructure.Events;
using AppGet.Installers;
using AppGet.Installers.Events;
using AppGet.PackageRepository;

namespace AppGet.Gui.Views.Installation
{
    public class InstallCommandViewModel : CommandViewModel<InstallOptions>
    {
        private readonly ICommandExecutor _executor;
        private readonly ITinyMessengerHub _hub;
        private readonly InitializingViewModel _initializingViewModel;
        private readonly DownloadProgressViewModel _installProgressViewModel;
        private readonly InstallingViewModel _installingViewModel;
        private readonly InstallationSuccessfulViewModel _installationSuccessfulViewModel;
        private TinyMessageSubscriptionToken _progressToken;

        public InstallCommandViewModel(ICommandExecutor executor, ITinyMessengerHub hub,
            InitializingViewModel initializingViewModel,
            DownloadProgressViewModel installProgressViewModel,
            InstallingViewModel installingViewModel,
            InstallationSuccessfulViewModel installationSuccessfulViewModel)
        {
            _executor = executor;
            _hub = hub;
            _initializingViewModel = initializingViewModel;
            _installProgressViewModel = installProgressViewModel;
            _installingViewModel = installingViewModel;
            _installationSuccessfulViewModel = installationSuccessfulViewModel;
        }

        protected override void OnActivate()
        {
            ActivateItem(_initializingViewModel);

            Task.Run(async () =>
            {
                try
                {
                    await _executor.ExecuteCommand(Options);
                }
                catch (PackageNotFoundException e)
                {
                    ShowError("Sorry, We couldn't find the package you were looking for.", $"Package ID: \"{e.PackageId}\"");
                }
                catch (InstallerException e)
                {
                    if (e.ExitReason.Category == ExitCodeTypes.RestartRequired)
                    {
                        ActivateItem(new RestartRequiredViewModel());
                        return;
                    }
                    var errorVm = new ErrorViewModel(
                        "Something strange has happened!",
                        $"Installer for {e.PackageManifest.Name} exited with a non-success status code of {e.ExitCode}:{e.ExitReason.Message}");
                    ActivateItem(errorVm);
                }
                catch (Exception e)
                {
                    var errorVm = new ErrorViewModel(e);
                    ActivateItem(errorVm);
                }
            });


            _hub.Subscribe<FileTransferStartedEvent>(e =>
            {
                ActivateItem(_installProgressViewModel);
            });

            _hub.Subscribe<ExecutingInstallerEvent>(e =>
            {
                ActivateItem(_installingViewModel);
            });

            _hub.Subscribe<InstallationSuccessfulEvent>(e =>
            {
                ActivateItem(_installationSuccessfulViewModel);
            });
        }
    }
}