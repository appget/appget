using System;
using System.Windows;
using Caliburn.Micro;
using JetBrains.Annotations;
using ModernUI;

namespace AppGet.Gui
{
    [UsedImplicitly]
    public partial class ShellView
    {
        private readonly IScreen _viewModel;

        public ShellView()
        {
            InitializeComponent();

            // don't do anything in design mode
            if (ModernUIHelper.IsInDesignMode)
            {
                return;
            }

            var view = (DependencyObject)System.Windows.Application.LoadComponent(new Uri(@"\CaliburnShellView.xaml", UriKind.Relative));

            Content = view;

            _viewModel = (IScreen)ViewModelLocator.LocateForView(view);
            ViewModelBinder.Bind(_viewModel, view, null);
            _viewModel.Activate();

            _viewModel.Deactivated += _viewModel_Deactivated;

            Closing += ShellView_Closing;
        }

        private void _viewModel_Deactivated(object sender, DeactivationEventArgs e)
        {
            _viewModel.Deactivated -= _viewModel_Deactivated;
            Close();
        }

        private void ShellView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _viewModel.Deactivated -= _viewModel_Deactivated;
            _viewModel.Deactivate(true);
        }
    }
}
