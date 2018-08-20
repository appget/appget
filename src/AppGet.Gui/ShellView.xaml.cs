using System;
using System.Windows;
using Caliburn.Micro;
using ModernUI;

namespace AppGet.Gui
{
    public partial class ShellView
    {
        public ShellView()
        {
            InitializeComponent();

            // don't do anything in design mode
            if (ModernUIHelper.IsInDesignMode)
            {
                return;
            }

            var view = (DependencyObject)System.Windows.Application.LoadComponent(new Uri(@"\CaliburnShellView.xaml", UriKind.Relative));
            var viewModel = ViewModelLocator.LocateForView(view);
            ViewModelBinder.Bind(viewModel, view, null);
            Content = view;
        }
    }
}
