using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.Extentions;
using CommandLine;

namespace AppGet.Options
{
    public abstract class AppGetOption
    {
        public RootOptions RootOptions { get; set; }

        public string CommandName { get; set; }

        [Option('v', null, HelpText = "Display more detailed execution information about")]
        public bool Verbose { get; set; }

        [ValueList(typeof(List<string>), MaximumElements = -1)]
        public IList<string> LeftOvers { get; set; }


        public virtual void ProcessArgs()
        {
            if (LeftOvers.Any())
            {
                throw new UnknownOptionException(LeftOvers);
            }
        }

        public abstract string GetUsage();

    }


    public abstract class PackageCommandOptions : AppGetOption
    {
        private static readonly Regex PackageIdRegex = new Regex("^\\w+", RegexOptions.Compiled);

        public string PackageId { get; private set; }

        public override void ProcessArgs()
        {
            var unknownArgs = new List<string>();

            for (int i = 0; i < LeftOvers.Count; i++)
            {
                var leftOver = LeftOvers[i];

                if (i == 0 && PackageIdRegex.IsMatch(leftOver))
                {
                    PackageId = leftOver;
                }
                else
                {
                    unknownArgs.Add(leftOver);
                }
            }

            if (unknownArgs.Any())
            {
                throw new UnknownOptionException(unknownArgs);
            }

            if (PackageId.IsNullOrWhiteSpace())
            {
                throw new PackageNameMissingException();
            }

        }

        public override string GetUsage()
        {
            return null;
        }
    }
}