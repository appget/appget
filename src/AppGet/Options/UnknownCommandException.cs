using System.Collections.Generic;

namespace AppGet.Options
{
    public class UnknownCommandException : OptionException
    {
        public UnknownCommandException(string command)
            : base("[{0}] is not a valid command.", command)
        {
        }
    }


    public class UnknownOptionException : OptionException
    {
        public IList<string> UnknownOptions { get; private set; }

        public UnknownOptionException(IList<string> unknownOptions)
            : base("unknown options")
        {
            UnknownOptions = unknownOptions;
        }
    }
}