using System.Collections.Generic;
using System.Linq;
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
}