using AppGet.Exceptions;

namespace AppGet.Manifests
{
    public class WindowsVersionParseException : AppGetException
    {
        public WindowsVersionParseException(string input)
            : base($"Unable to parse [{input}] into a valid Windows versions")
        {
        }
    }
}