using System.Collections.Generic;
using System.Linq;

namespace AppGet.Options
{
    public class UnknownOptionException : OptionException
    {
        public IList<string> UnknownOptions { get; private set; }

        public UnknownOptionException(IList<string> unknownOptions)
            : base(BuildMessage(unknownOptions))
        {
            UnknownOptions = unknownOptions;
        }

        private static string BuildMessage(IEnumerable<string> unknownOptions)
        {
            return "invalid options: " + string.Join(", ", unknownOptions.ToArray());
        }
    }
}