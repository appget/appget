using System.Collections.Generic;

namespace AppGet
{
    public class Arguments
    {
        public Arguments(string arguments)
        {

        }

        public string Command { get; private set; }
        public HashSet<string> Flags { get; private set; }
        public Dictionary<string, string> Params { get; private set; }
    }
}