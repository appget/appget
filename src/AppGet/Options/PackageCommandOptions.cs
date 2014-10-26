using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.Extentions;
using CommandLine;

namespace AppGet.Options
{
    public abstract class AppGetOption
    {
        public string CommandName { get; set; }

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
        private static readonly Regex PackageNameRegex = new Regex("^\\w+", RegexOptions.Compiled);

        public string PackageName { get; private set; }

        public override void ProcessArgs()
        {
            var unknownArgs = new List<string>();

            for (int i = 0; i < LeftOvers.Count; i++)
            {
                var leftOver = LeftOvers[i];

                if (i == 0 && PackageNameRegex.IsMatch(leftOver))
                {
                    PackageName = leftOver;
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

            if (PackageName.IsNullOrWhiteSpace())
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