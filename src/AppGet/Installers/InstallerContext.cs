using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppGet.HostSystem;
using AppGet.Installers.UninstallerWhisperer;
using AppGet.Windows.WindowsInstaller;

namespace AppGet.Installers
{
    public class ChangeSet<T>
    {
        public List<T> Added { get; set; }
        public List<T> Removed { get; set; }
        public List<T> Updated { get; set; }
    }

    public class InstallerReport
    {
        public ChangeSet<WindowsInstallerRecord> InstallerRecords { get; set; }
        public string PackageId { get; set; }
        public int ExitCode { get; set; }
        public InstallInteractivityLevel Interactivity { get; set; }
        public double Duration { get; set; }
        public bool IsAdmin { get; set; }
        public string ErrorCat { get; set; }
        public bool IsGui { get; set; }
        public string Engine { get; set; }
    }

    public class InstallerContext : IDisposable
    {
        private readonly string _packageId;
        private readonly WindowsInstallerClient _installerClient;
        private readonly NexClient _nexClient;
        private readonly Stopwatch _stopWatch;
        private readonly List<WindowsInstallerRecord> _preOperationInstallRecords;
        public InstallInteractivityLevel InteractivityLevel { get; }

        public InstallerContext(string packageId, InstallInteractivityLevel interactivityLevel, WindowsInstallerClient installerClient, NexClient nexClient)
        {
            _packageId = packageId;
            _installerClient = installerClient;
            _nexClient = nexClient;
            InteractivityLevel = interactivityLevel;


            _preOperationInstallRecords = installerClient.GetRecords();
            _stopWatch = Stopwatch.StartNew();
        }

        public Process Process { get; set; }
        public List<WindowsInstallerRecord> InstallerRecords { get; set; }
        public InstallerException Exception { get; set; }
        public PackageOperation Operation { get; set; }
        public IInstaller Whisperer { get; set; }

        private void SubmitResult()
        {
            var postOperationInstallRecords = _installerClient.GetRecords();
            var diff = new DiffGenerator<WindowsInstallerRecord>(_preOperationInstallRecords, postOperationInstallRecords, c => $"{c.Is64}_{c.Id}");

            var envInfo = new EnvInfo();

            var report = new InstallerReport
            {
                PackageId = _packageId,
                InstallerRecords = new ChangeSet<WindowsInstallerRecord>
                {
                    Added = diff.Added().ToList(),
                    Removed = diff.Removed().ToList(),
                    Updated = diff.Updated().ToList()
                },
                Interactivity = InteractivityLevel,
                Duration = _stopWatch.Elapsed.TotalSeconds,
                Engine = Whisperer?.InstallMethod.ToString(),
                IsAdmin = envInfo.IsAdministrator,
                IsGui = envInfo.IsGui,
            };


            if (Process != null)
            {
                report.ExitCode = Process.ExitCode;
            }

            if (Exception != null)
            {
                report.ExitCode = Exception.ExitCode;
                report.ErrorCat = Exception.ExitReason.Category.ToString();
            }

            _nexClient.SubmitReport(report).Wait();
        }


        public void Dispose()
        {
            _stopWatch.Stop();
            SubmitResult();
        }
    }
}
