using System;
using System.IO;
using AppGet.HostSystem;
using AppGet.Processes;
using Microsoft.Win32;
using NLog;

namespace AppGet.Tools
{
    public interface ISigCheck
    {
        string GetManifest(string path);
    }

    public class SigCheck : ISigCheck
    {
        private readonly IProcessController _processController;
        private readonly Logger _logger;
        private readonly string _sigCheckPath;

        private static bool _eulaAccepted;

        public SigCheck(IProcessController processController, IEnvInfo envInfo, Logger logger)
        {
            _processController = processController;
            _logger = logger;
            _sigCheckPath = Path.Combine(envInfo.AppDir, "sigcheck.exe");
        }

        private void AcceptEula()
        {
            if (_eulaAccepted) return;

            const string SIG_CHECK_KEY_PATH = "Software\\Sysinternals\\Sigcheck";
            const string EULA_ACCEPTED_KEY = "EulaAccepted";

            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(SIG_CHECK_KEY_PATH, true))
                {
                    if (key == null)
                    {
                        using (var newSubKey = Registry.CurrentUser.CreateSubKey(SIG_CHECK_KEY_PATH))
                        {
                            newSubKey.SetValue(EULA_ACCEPTED_KEY, 1);
                        }
                    }
                    else
                    {
                        key.SetValue(EULA_ACCEPTED_KEY, 1);
                    }

                    _eulaAccepted = true;
                }
            }
            catch (Exception e)
            {
                _logger.Debug(e, "Couldn't accept sigcheck.exe EULA");
            }
        }


        public string GetManifest(string path)
        {
            AcceptEula();

            var process = _processController.Start(_sigCheckPath, $"-m -nobanner \"{path}\"", false);

            process.WaitForExit();

            var output = "";

            string line;
            while ((line = process.StandardOutput.ReadLine()) != null)
            {
                if (!line.ToLowerInvariant().Contains(path.ToLowerInvariant()))
                {
                    output += line + Environment.NewLine;
                }
            }

            return output;
        }
    }
}
