using System.Collections.Generic;
using System.Diagnostics;
using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Msi
{
    public class MsiWhisperer : InstallerWhispererBase
    {
        // http://msdn.microsoft.com/library/aa368542.aspx
        public override Dictionary<int, ExistReason> ExitCodes => new Dictionary<int, ExistReason>
        {
            {
                13, new ExistReason(ExitCodeTypes.Failed, "ERROR_INVALID_DATA")
            },
            {
                87, new ExistReason(ExitCodeTypes.Failed, "ERROR_INVALID_PARAMETER")
            },
            {
                120,
                new ExistReason(ExitCodeTypes.Failed,
                    "ERROR_CALL_NOT_IMPLEMENTED: This value is returned when a custom action attempts to call a function that cannot be called from custom actions.")
            },
            {
                1259, new ExistReason(ExitCodeTypes.RequirementUnmet)
            },
            {
                1601,
                new ExistReason(ExitCodeTypes.Failed,
                    "The Windows Installer service could not be accessed. Contact your support personnel to verify that the Windows Installer service is properly registered.")
            },
            {
                1602, new ExistReason(ExitCodeTypes.UserCanceled)
            },
            {
                1603, new ExistReason(ExitCodeTypes.Failed, "ERROR_INSTALL_FAILURE: A fatal error occurred during installation.")
            },
            {
                1604, new ExistReason(ExitCodeTypes.Failed, "Installation suspended, incomplete.")
            },
            {
                1605, new ExistReason(ExitCodeTypes.Failed, "ERROR_UNKNOWN_PRODUCT: This action is only valid for products that are currently installed.")
            },
            {
                1606, new ExistReason(ExitCodeTypes.Failed, "ERROR_UNKNOWN_FEATURE: The feature identifier is not registered.")
            },
            {
                1607, new ExistReason(ExitCodeTypes.Failed, "ERROR_UNKNOWN_COMPONENT: The component identifier is not registered.")
            },
            {
                1608, new ExistReason(ExitCodeTypes.Failed, "ERROR_UNKNOWN_PROPERTY")
            },
            {
                1609, new ExistReason(ExitCodeTypes.Failed, "ERROR_INVALID_HANDLE_STATE")
            },
            {
                1610,
                new ExistReason(ExitCodeTypes.Failed,
                    "ERROR_BAD_CONFIGURATION: The configuration data for this product is corrupt. Contact your support personnel.")
            },
            {
                1611, new ExistReason(ExitCodeTypes.Failed, "ERROR_INDEX_ABSENT: The component qualifier not present.")
            },
            {
                1612,
                new ExistReason(ExitCodeTypes.Failed,
                    "ERROR_INSTALL_SOURCE_ABSENT: The installation source for this product is not available. Verify that the source exists and that you can access it.")
            },
            {
                1613,
                new ExistReason(ExitCodeTypes.RequirementUnmet,
                    "This installation package cannot be installed by the Windows Installer service. You must install a Windows service pack that contains a newer version of the Windows Installer service.")
            },
            {
                1614, new ExistReason(ExitCodeTypes.Failed, "ERROR_PRODUCT_UNINSTALLED: The product is uninstalled.")
            },
            {
                1615, new ExistReason(ExitCodeTypes.Failed, "ERROR_BAD_QUERY_SYNTAX: The SQL query syntax is invalid or unsupported.")
            },
            {
                1616, new ExistReason(ExitCodeTypes.Failed, "ERROR_INVALID_FIELD: The record field does not exist.")
            },
            {
                1618,
                new ExistReason(ExitCodeTypes.Failed,
                    "Another installation is already in progress. Complete that installation before proceeding with this install.")
            },
            {
                1619,
                new ExistReason(ExitCodeTypes.Failed,
                    "This installation package could not be opened. Verify that the package exists and is accessible, or contact the application vendor to verify that this is a valid Windows Installer package.")
            },
            {
                1620,
                new ExistReason(ExitCodeTypes.Failed,
                    "This installation package could not be opened. Contact the application vendor to verify that this is a valid Windows Installer package.")
            },
            {
                1621,
                new ExistReason(ExitCodeTypes.Failed,
                    "There was an error starting the Windows Installer service user interface. Contact your support personnel.")
            },
            {
                1622,
                new ExistReason(ExitCodeTypes.Failed,
                    "There was an error opening installation log file. Verify that the specified log file location exists and is writable.")
            },
            {
                1623, new ExistReason(ExitCodeTypes.Failed, "This language of this installation package is not supported by your system.")
            },
            {
                1624, new ExistReason(ExitCodeTypes.Failed, "ERROR_INSTALL_TRANSFORM_FAILURE")
            },
            {
                1625, new ExistReason(ExitCodeTypes.Failed, "This installation is forbidden by system policy. Contact your system administrator.")
            },
            {
                1626, new ExistReason(ExitCodeTypes.Failed, "ERROR_FUNCTION_NOT_CALLED")
            },
            {
                1627, new ExistReason(ExitCodeTypes.Failed, "ERROR_FUNCTION_FAILED")
            },
            {
                1628, new ExistReason(ExitCodeTypes.Failed, "ERROR_INVALID_TABLE")
            },
            {
                1629, new ExistReason(ExitCodeTypes.Failed, "ERROR_DATATYPE_MISMATCH")
            },
            {
                1630, new ExistReason(ExitCodeTypes.Failed, "ERROR_UNSUPPORTED_TYPE")
            },
            {
                1631,
                new ExistReason(ExitCodeTypes.Failed, "ERROR_CREATE_FAILED: The Windows Installer service failed to start. Contact your support personnel.")
            },
            {
                1632,
                new ExistReason(ExitCodeTypes.Failed,
                    "The Temp folder is either full or inaccessible. Verify that the Temp folder exists and that you can write to it.")
            },
            {
                1633, new ExistReason(ExitCodeTypes.Failed, "This installation package is not supported on this platform.")
            },
            {
                1634, new ExistReason(ExitCodeTypes.Failed, "ERROR_INSTALL_NOTUSED: Component is not used on this machine.")
            },
            {
                1635,
                new ExistReason(ExitCodeTypes.CorruptInstaller,
                    "This patch package could not be opened. Verify that the patch package exists and is accessible and is a valid Windows Installer patch package.")
            },
            {
                1636,
                new ExistReason(ExitCodeTypes.CorruptInstaller,
                    "This patch package could not be opened. Verify that this is a valid Windows Installer patch package.")
            },
            {
                1637,
                new ExistReason(ExitCodeTypes.RequirementUnmet,
                    "This patch package cannot be processed by the Windows Installer service. You must install a Windows service pack that contains a newer version of the Windows Installer service.")
            },
            {
                1638,
                new ExistReason(ExitCodeTypes.RequirementUnmet,
                    "Another version of this product is already installed. Installation of this version cannot continue.")
            },
            {
                1639, new ExistReason(ExitCodeTypes.RequirementUnmet, "ERROR_INVALID_COMMAND_LINE")
            },
            {
                1640, new ExistReason(ExitCodeTypes.RequirementUnmet, "ERROR_INSTALL_REMOTE_DISALLOWED")
            },
            {
                1641, new ExistReason(ExitCodeTypes.RestartRequired, "The installer has initiated a restart.", true)
            },
            {
                1642,
                new ExistReason(ExitCodeTypes.RestartRequired,
                    "The installer cannot install the upgrade patch because the program being upgraded may be missing or the upgrade patch updates a different version of the program. Verify that the program to be upgraded exists on your computer and that you have the correct upgrade patch. ")
            },
            {
                1643, new ExistReason(ExitCodeTypes.RestartRequired, "The patch package is not permitted by system policy.")
            },
            {
                1644, new ExistReason(ExitCodeTypes.RestartRequired, "One or more customizations are not permitted by system policy.")
            },
            {
                1645, new ExistReason(ExitCodeTypes.RestartRequired, "ERROR_INSTALL_REMOTE_PROHIBITED")
            },
            {
                1646, new ExistReason(ExitCodeTypes.RestartRequired, "The patch package is not a removable patch package.")
            },
            {
                1647, new ExistReason(ExitCodeTypes.RestartRequired, "The patch is not applied to this product.")
            },
            {
                1648, new ExistReason(ExitCodeTypes.RestartRequired, "ERROR_PATCH_NO_SEQUENCE")
            },
            {
                1649, new ExistReason(ExitCodeTypes.RestartRequired, "Patch removal was disallowed by policy.")
            },
            {
                1650, new ExistReason(ExitCodeTypes.RestartRequired, "ERROR_INVALID_PATCH_XML")
            },
            {
                1651,
                new ExistReason(ExitCodeTypes.RestartRequired,
                    "Administrative user failed to apply patch for a per-user managed or a per-machine application that is in advertise state.")
            },
            {
                1652,
                new ExistReason(ExitCodeTypes.RestartRequired,
                    "Windows Installer is not accessible when the computer is in Safe Mode. Exit Safe Mode and try again or try using System Restore to return your computer to a previous state.")
            },
            {
                1653,
                new ExistReason(ExitCodeTypes.RestartRequired,
                    "Could not perform a multiple-package transaction because rollback has been disabled. Multiple-Package Installations cannot run if rollback is disabled.")
            },
            {
                1654, new ExistReason(ExitCodeTypes.RestartRequired, "The app that you are trying to run is not supported on this version of Windows.")
            },
            {
                3010, new ExistReason(ExitCodeTypes.RestartRequired, null, true)
            },

            // https://docs.microsoft.com/en-us/dotnet/framework/deployment/guide-for-administrators#return-codes
            {
                5100, new ExistReason(ExitCodeTypes.RequirementUnmet, "The user's computer does not meet system requirements.")
            },
        };

        public MsiWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }

        protected override InstallMethodTypes InstallMethod => InstallMethodTypes.MSI;

        protected override Process StartProcess(string installerLocation, string args)
        {
            return base.StartProcess("msiexec", $"/i \"{installerLocation}\" {args}");
        }

        protected override string InteractiveArgs => "/qf";
        protected override string PassiveArgs => "/qb /norestart";
        protected override string SilentArgs => "/qn /norestart";
        protected override string LogArgs => "/L* {path}";
    }
}