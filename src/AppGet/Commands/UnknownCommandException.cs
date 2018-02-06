using AppGet.Exceptions;
using AppGet.Options;

namespace AppGet.Commands
{
    public class UnknownCommandException : AppGetException
    {
        public UnknownCommandException(AppGetOption option) :
            base($"Couldn't find handler for {option.GetType().Name}")
        {
        }
    }
}