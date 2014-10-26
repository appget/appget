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

        public string PackageName { get; private set; }
        public List<string> UnknownArgs { get; private set; }

        public void ProcessUnknownArgs()
        {
            UnknownArgs = new List<string>();

            for (int i = 0; i < LeftOvers.Count; i++)
            {
                var leftOver = LeftOvers[i];

                if (i == 0 && PackageNameRegex.IsMatch(leftOver))
                {
                    PackageName = leftOver;
                }
                else
                {
                    UnknownArgs.Add(leftOver);
                }
            }
        }
    }
}