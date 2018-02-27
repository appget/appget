using System;
using System.IO;
using System.Linq;
using AppGet.HostSystem;
using AppGet.Processes;

namespace AppGet.Tools
{
    public interface ISigCheck
    {
        string GetManifest(string path);
    }

    public class SigCheck : ISigCheck
    {
        private readonly IProcessController _processController;
        private readonly string _sigCheckPath;

        public SigCheck(IProcessController processController, IEnvInfo envInfo)
        {
            _processController = processController;
            _sigCheckPath = Path.Combine(envInfo.AppDir, "sigcheck.exe");
        }

        public string GetManifest(string path)
        {
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
