using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.Extentions;

namespace AppGet.Options
{
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