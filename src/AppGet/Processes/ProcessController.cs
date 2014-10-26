using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NLog;

namespace AppGet.Processes
{
    public class ProcessOutput
    {
        public List<String> Standard { get; set; }
        public List<String> Error { get; set; }

        public ProcessOutput()
        {
            Standard = new List<string>();
            Error = new List<string>();
        }
    }

    public class ProcessInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartPath { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1} [{2}]", Id, Name ?? "Unknown", StartPath ?? "Unknown");
        }
    }
    public interface IProcessController
    {
        ProcessInfo GetCurrentProcess();
        ProcessInfo GetProcessById(int id);
        List<ProcessInfo> FindProcessByName(string name);
        void OpenDefaultBrowser(string url);
        void WaitForExit(Process process);
        void KillAll(string processName);
        void Kill(int processId);
        Boolean Exists(int processId);
        Boolean Exists(string processName);
        Process Start(string path, string args = null, Action<string> onOutputDataReceived = null, Action<string> onErrorDataReceived = null);
        Process SpawnNewProcess(string path, string args = null);
        ProcessOutput StartAndCapture(string path, string args = null);
    }

    public class ProcessController : IProcessController
    {
        private readonly Logger _logger;

        public ProcessController(Logger logger)
        {
            _logger = logger;
        }

        public ProcessInfo GetCurrentProcess()
        {
            return ConvertToProcessInfo(Process.GetCurrentProcess());
        }

        public bool Exists(int processId)
        {
            return GetProcessById(processId) != null;
        }

        public bool Exists(string processName)
        {
            return GetProcessesByName(processName).Any();
        }

        public ProcessInfo GetProcessById(int id)
        {
            _logger.Debug("Finding process with Id:{0}", id);

            var processInfo = ConvertToProcessInfo(Process.GetProcesses().FirstOrDefault(p => p.Id == id));

            if (processInfo == null)
            {
                _logger.Warn("Unable to find process with ID {0}", id);
            }
            else
            {
                _logger.Debug("Found process {0}", processInfo.ToString());
            }

            return processInfo;
        }

        public List<ProcessInfo> FindProcessByName(string name)
        {
            return GetProcessesByName(name).Select(ConvertToProcessInfo).Where(c => c != null).ToList();
        }

        public void OpenDefaultBrowser(string url)
        {
            _logger.Info("Opening URL [{0}]", url);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo(url)
                {
                    UseShellExecute = true
                }
            };

            process.Start();
        }

        public Process Start(string path, string args = null, Action<string> onOutputDataReceived = null, Action<string> onErrorDataReceived = null)
        {
            var logger = LogManager.GetLogger(new FileInfo(path).Name);

            var startInfo = new ProcessStartInfo(path, args)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };


            logger.Debug("Starting {0} {1}", path, args);

            var process = new Process
            {
                StartInfo = startInfo
            };

            process.OutputDataReceived += (sender, eventArgs) =>
            {
                if (string.IsNullOrEmpty(eventArgs.Data)) return;

                logger.Debug(eventArgs.Data);

                if (onOutputDataReceived != null)
                {
                    onOutputDataReceived(eventArgs.Data);
                }
            };

            process.ErrorDataReceived += (sender, eventArgs) =>
            {
                if (string.IsNullOrEmpty(eventArgs.Data)) return;

                logger.Error(eventArgs.Data);

                if (onErrorDataReceived != null)
                {
                    onErrorDataReceived(eventArgs.Data);
                }
            };

            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            return process;
        }

        public Process SpawnNewProcess(string path, string args = null)
        {
            _logger.Debug("Starting {0} {1}", path, args);

            var startInfo = new ProcessStartInfo(path, args);
            var process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();

            return process;
        }

        public ProcessOutput StartAndCapture(string path, string args = null)
        {
            var output = new ProcessOutput();
            Start(path, args, s => output.Standard.Add(s), error => output.Error.Add(error)).WaitForExit();

            return output;
        }

        public void WaitForExit(Process process)
        {
            _logger.Debug("Waiting for process {0} to exit.", process.ProcessName);

            process.WaitForExit();
        }


        public void Kill(int processId)
        {
            var process = Process.GetProcesses().FirstOrDefault(p => p.Id == processId);

            if (process == null)
            {
                _logger.Warn("Cannot find process with id: {0}", processId);
                return;
            }

            process.Refresh();

            if (process.HasExited)
            {
                _logger.Debug("Process has already exited");
                return;
            }

            _logger.Info("[{0}]: Killing process", process.Id);
            process.Kill();
            _logger.Info("[{0}]: Waiting for exit", process.Id);
            process.WaitForExit();
            _logger.Info("[{0}]: Process terminated successfully", process.Id);
        }

        public void KillAll(string processName)
        {
            var processes = GetProcessesByName(processName);

            _logger.Debug("Found {0} processes to kill", processes.Count);

            foreach (var processInfo in processes)
            {
                _logger.Debug("Killing process: {0} [{1}]", processInfo.Id, processInfo.ProcessName);
                Kill(processInfo.Id);
            }
        }

        private ProcessInfo ConvertToProcessInfo(Process process)
        {
            if (process == null) return null;

            process.Refresh();

            ProcessInfo processInfo = null;

            try
            {
                if (process.Id <= 0) return null;

                processInfo = new ProcessInfo();
                processInfo.Id = process.Id;
                processInfo.Name = process.ProcessName;
                processInfo.StartPath = process.MainModule.FileName;

                if (process.HasExited)
                {
                    processInfo = null;
                }
            }
            catch (Win32Exception e)
            {
                _logger.Warn("Couldn't get process info for " + process.ProcessName, e);
            }

            return processInfo;

        }



        private List<Process> GetProcessesByName(string name)
        {
            var processes = Process.GetProcessesByName(name).ToList();
            _logger.Debug("Found {0} processes with the name: {1}", processes.Count, name);
            return processes;
        }
    }
}
