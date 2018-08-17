using System.Diagnostics;
using System.IO;
using System.Linq;
using NLog;

namespace AppGet.Windows
{
    public interface IProcessController
    {
        Process Start(string fileName, string args = null, bool useShellExecute = true);
        void Kill(Process process, int timeout);
        void WaitForExit(Process process, int? timeout = null);
        Process TryGetRunningProcess(int processId);
    }

    public class ProcessController : IProcessController
    {
        private readonly Logger _logger;

        public ProcessController(Logger logger)
        {
            _logger = logger;
        }

        public Process Start(string fileName, string args = null, bool useShellExecute = true)
        {
            var logger = LogManager.GetLogger(new FileInfo(fileName).Name);

            var startInfo = new ProcessStartInfo(fileName, args)
            {
                CreateNoWindow = true,
                UseShellExecute = useShellExecute,
                RedirectStandardOutput = !useShellExecute,
                RedirectStandardError = !useShellExecute
            };

            logger.Debug("Starting {0} {1}", fileName, args);

            var process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();

            return process;
        }

        public Process TryGetRunningProcess(int processId)
        {
            return Process.GetProcesses().FirstOrDefault(c => c.Id == processId && !c.HasExited);
        }

        public void Kill(Process process, int timeout)
        {
            _logger.Info("Terminating process '{0}'", process.ProcessName);
            process.Kill();
            WaitForExit(process, timeout);
        }

        public void WaitForExit(Process process, int? timeout)
        {
            _logger.Debug("Waiting for process '{0}' to exit", process.ProcessName);

            if (timeout.HasValue)
            {
                process.WaitForExit(timeout.Value);
            }
            else
            {
                process.WaitForExit();
            }
        }
    }
}