using System.Diagnostics;
using System.IO;
using NLog;

namespace AppGet.Processes
{
    public interface IProcessController
    {
        void WaitForExit(Process process);
        Process Start(string path, string args = null, bool useShellExecute = true);
    }

    public class ProcessController : IProcessController
    {
        private readonly Logger _logger;

        public ProcessController(Logger logger)
        {
            _logger = logger;
        }

        public Process Start(string path, string args = null, bool useShellExecute = true)
        {
            var logger = LogManager.GetLogger(new FileInfo(path).Name);

            var startInfo = new ProcessStartInfo(path, args)
            {
                CreateNoWindow = true,
                UseShellExecute = useShellExecute,
                RedirectStandardOutput = !useShellExecute,
                RedirectStandardError = !useShellExecute
            };

            logger.Debug("Starting {0} {1}", path, args);

            var process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();

            return process;
        }

        public void WaitForExit(Process process)
        {
            _logger.Debug("Waiting for process {0} to exit", process.ProcessName);

            process.WaitForExit();
        }
    }
}
