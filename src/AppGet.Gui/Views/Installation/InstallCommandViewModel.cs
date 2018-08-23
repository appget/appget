using System;
using System.ComponentModel;
using System.Threading.Tasks;
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
    public class InstallCommandViewModel : CommandViewModel<InstallOptions>,
        IHandle<FileTransferStartedEvent>,
        IHandle<ExecutingInstallerEvent>,
        IHandle<InstallationSuccessfulEvent>
    {
        private readonly ICommandExecutor _executor;
        private readonly InitializingViewModel _initializingViewModel;
        private readonly DownloadProgressViewModel _installProgressViewModel;
        private readonly InstallingViewModel _installingViewModel;
        private readonly InstallationSuccessfulViewModel _installationSuccessfulViewModel;

        public InstallCommandViewModel(ICommandExecutor executor,
            InitializingViewModel initializingViewModel,
            DownloadProgressViewModel installProgressViewModel,
            InstallingViewModel installingViewModel,
            InstallationSuccessfulViewModel installationSuccessfulViewModel)
        {
            _executor = executor;
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
                    ActivateItem(e.CreateDialog());
                }
                catch (InstallerException e)
                {
                    ActivateItem(e.CreateDialog());
                }
                catch (Win32Exception e)
                {
                    ActivateItem(e.CreateDialog());
                }
                catch (Exception e)
                {
                    ActivateItem(e.CreateDialog());
                }
            });
        }

        public void Handle(FileTransferStartedEvent message)
        {
            ActivateItem(_installProgressViewModel);
        }

        public void Handle(ExecutingInstallerEvent message)
        {
            ActivateItem(_installingViewModel);
        }

        public void Handle(InstallationSuccessfulEvent message)
        {
            ActivateItem(_installationSuccessfulViewModel);
        }
    }
}