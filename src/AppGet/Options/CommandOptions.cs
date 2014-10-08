using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CommandLine;

namespace AppGet.Options
{
    public abstract class CommandOptions
    {

        private static readonly Regex PackageNameRegex = new Regex("^\\w+", RegexOptions.Compiled);

        public string CommandName { get; set; }

        [ValueList(typeof(List<string>), MaximumElements = -1)]
        public IList<string> LeftOvers { get; set; }

        public List<string> Packages { get; set; }
        public List<string> UnknowArgs { get; set; }

        public void ProcessUnknowArgs()
        {
            Packages = new List<string>();
            UnknowArgs = new List<string>();
            foreach (var leftOver in LeftOvers)
            {
                if (PackageNameRegex.IsMatch(leftOver) && !UnknowArgs.Any())
                {
                    Packages.Add(leftOver);
                }
                else
                {
                    UnknowArgs.Add(leftOver);
                }
            }
        }
    }
}