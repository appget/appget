using System;
using System.ComponentModel;
using System.Threading.Tasks;
using AppGet.Commands;
using AppGet.Commands.Install;
using AppGet.FileTransfer;
using AppGet.Gui.Views.Shared;
using AppGet.Infrastructure.Eventing;
using AppGet.Installers;
using AppGet.Installers.Events;
using AppGet.PackageRepository;

namespace AppGet.Gui.Views.Installation
{
    public class InstallCommandViewModel : CommandViewModel<InstallOptions>
    {
        private readonly ICommandExecutor _executor;
        private readonly IHub _hub;
        private readonly InstallationConsentViewModel _consentViewModel;
        private readonly InitializingViewModel _initializingViewModel;
        private readonly DownloadProgressViewModel _installProgressViewModel;
        private readonly InstallingViewModel _installingViewModel;
        private readonly InstallationSuccessfulViewModel _installationSuccessfulViewModel;

        public InstallCommandViewModel(ICommandExecutor executor,
            IHub hub,
            InstallationConsentViewModel _consentViewModel,
            InitializingViewModel initializingViewModel,
            DownloadProgressViewModel installProgressViewModel,
            InstallingViewModel installingViewModel,
            InstallationSuccessfulViewModel installationSuccessfulViewModel)
        {
            _executor = executor;
            _hub = hub;
            this._consentViewModel = _consentViewModel;
            _initializingViewModel = initializingViewModel;
            _installProgressViewModel = installProgressViewModel;
            _installingViewModel = installingViewModel;
            _installationSuccessfulViewModel = installationSuccessfulViewModel;
        }

        protected override void OnInitialize()
        {
            _hub.Subscribe<FileTransferStartedEvent>(this, e =>
            {
                ActivateItem(_installProgressViewModel);
            });

            _hub.Subscribe<FileTransferCompletedEvent>(this, e =>
            {
                ActivateItem(_installingViewModel);
            });

            _hub.Subscribe<InstallationSuccessfulEvent>(this, e =>
            {
                ActivateItem(_installationSuccessfulViewModel);
            });

            Deactivated += (sender, args) =>
            {
                _hub.UnSubscribe(this);
            };
        }

        protected override void OnActivate()
        {
            ActivateItem(_consentViewModel);
        }

        public void Install()
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
    }
}