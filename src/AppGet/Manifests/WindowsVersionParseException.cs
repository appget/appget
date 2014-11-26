using AppGet.Exceptions;

namespace AppGet.Manifests
{
    public class WindowsVersionParseException : AppGetException
    {
        public WindowsVersionParseException(string input)
            : base("Unbale to parse [{0}] into a valid Windows versions", input)
        {
        }
    }
}