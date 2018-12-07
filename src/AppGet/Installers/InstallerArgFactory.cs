using System;
using AppGet.HostSystem;
using AppGet.Installers.UninstallerWhisperer;
using AppGet.Manifest;
using NLog;

namespace AppGet.Installers
{

    public class InstallerArguments
    {
        public string Arguments { get; set; }
        public string LogFile { get; set; }
    }

    public class InstallerArgFactory
    {
        private readonly Logger _logger;
        private readonly IPathResolver _pathResolver;

        public InstallerArgFactory(Logger logger, IPathResolver pathResolver)
        {
            _logger = logger;
            _pathResolver = pathResolver;
        }


        public InstallerArguments GetInstallerArguments(InstallInteractivityLevel interactivity, PackageManifest manifest, IInstaller installer)
        {
            var result = new InstallerArguments
            {
                LogFile = _pathResolver.GetInstallerLogFile(manifest.Id)
            };

            var effectiveInteractivity = GetInteractivelyLevel(interactivity, manifest.Args, installer);

            switch (effectiveInteractivity)
            {
                case InstallInteractivityLevel.Silent:
                    result.Arguments = $"{installer.SilentArgs} {manifest.Args?.Silent}";
                    break;

                case InstallInteractivityLevel.Interactive:
                    result.Arguments = $"{installer.InteractiveArgs} {manifest.Args?.Interactive}";
                    break;

                case InstallInteractivityLevel.Passive:
                    result.Arguments = $"{installer.PassiveArgs} {manifest.Args?.Passive}";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(effectiveInteractivity));
            }

            var logArg = GetLoggingArgs(result.LogFile, manifest.Args, installer);

            if (string.IsNullOrWhiteSpace(logArg))
            {
                result.LogFile = null;
            }
            else
            {
                _logger.Info($"Writing installer log files to {result.LogFile}");
                result.Arguments = $"{result.Arguments.Trim()} {logArg.Trim()}";
            }

            return result;
        }

        private string GetLoggingArgs(string logFile, InstallArgs args, IInstaller installer)
        {
            var template = args?.Log ?? installer.LogArgs;
            return template?.Replace("{path}", $"\"{logFile}\"");
        }

        private InstallInteractivityLevel GetInteractivelyLevel(InstallInteractivityLevel interactivity, InstallArgs args, IInstaller installer)
        {
            bool SupportsSilent()
            {
                return args?.Silent != null || installer.SilentArgs != null;
            }

            bool SupportsPassive()
            {
                return args?.Passive != null || installer.PassiveArgs != null;
            }

            if (interactivity == InstallInteractivityLevel.Silent && !SupportsSilent())
            {
                if (SupportsPassive())
                {
                    _logger.Info("Silent install is not supported by installer. Switching to Passive");
                    return InstallInteractivityLevel.Passive;
                }

                _logger.Warn("Silent or Passive install is not supported by installer. Switching to Interactive");
                return InstallInteractivityLevel.Interactive;
            }

            if (interactivity == InstallInteractivityLevel.Passive && !SupportsPassive())
            {
                if (SupportsSilent())
                {
                    _logger.Info("Passive install is not supported by installer. Switching to Silent.");
                    return InstallInteractivityLevel.Silent;
                }

                _logger.Warn("Silent or Passive install is not supported by installer. Switching to Interactive");
                return InstallInteractivityLevel.Interactive;
            }

            return interactivity;
        }
    }
}
