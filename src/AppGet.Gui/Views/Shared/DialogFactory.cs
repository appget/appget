using System;
using System.ComponentModel;
using AppGet.Gui.Controls;
using AppGet.Installers;
using AppGet.PackageRepository;

namespace AppGet.Gui.Views.Shared
{
    public static class DialogFactory
    {
        public static DialogViewModel CreateDialog(this Exception ex)
        {
            var headerVm = new DialogHeaderViewModel(ex.GetType().Name.Replace("Exception", ""), ex.Message, "sad-cry", Accents.Error);
            return new DialogViewModel(headerVm);
        }

        public static DialogViewModel CreateDialog(this PackageNotFoundException ex)
        {
            var headerVm = new DialogHeaderViewModel("Sorry, We couldn't find the package you were looking for.", $"Package ID: \"{ex.PackageId}\"", "crow", Accents.Warn);
            return new DialogViewModel(headerVm);
        }


        public static DialogViewModel CreateDialog(this Win32Exception ex)
        {
            var headerVm = new DialogHeaderViewModel("Microsoft Windows", ex.Message, "window-restore", Accents.Warn);
            return new DialogViewModel(headerVm);
        }

        public static DialogViewModel CreateDialog(this InstallerException ex)
        {
            var exitCode = $"ExitCode:{ex.ExitCode}";

            switch (ex.ExitReason?.Category)
            {
                case ExitCodeTypes.CorruptInstaller:
                    {
                        var headerVm = new DialogHeaderViewModel($"Installer for {ex.PackageManifest.Name} is corrupted.",
                            exitCode,
                            "exclamation",
                            Accents.Warn);
                        return new DialogViewModel(headerVm);
                    }
                case ExitCodeTypes.RequirementUnmet:
                    {
                        var headerVm = new DialogHeaderViewModel("Installer has reported that your system doesn\'t meet one of it\'s requirements.",
                            exitCode,
                            "exclamation",
                            Accents.Warn);
                        return new DialogViewModel(headerVm);
                    }
                case ExitCodeTypes.RestartRequired:
                    {
                        var headerVm = new DialogHeaderViewModel($"{ex.PackageManifest.Name} has requested a system restart to finish installation.",
                            "",
                            "undo",
                            Accents.Warn);
                        return new DialogViewModel(headerVm);
                    }
                case ExitCodeTypes.UserCanceled:
                    {
                        var headerVm = new DialogHeaderViewModel($"User has canceled the installation",
                            "But you probably knew that :)",
                            "undo",
                            Accents.Info);
                        return new DialogViewModel(headerVm);
                    }
            }

            return new DialogViewModel(new DialogHeaderViewModel($"Installer for {ex.PackageManifest.Name} exited with a non-success status code",
                exitCode, "exclamation", Accents.Error));

        }
    }
}