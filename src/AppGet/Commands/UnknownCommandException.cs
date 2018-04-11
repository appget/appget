using AppGet.Exceptions;

namespace AppGet.Commands
{
    public class UnknownCommandException : AppGetException
    {
        public UnknownCommandException(AppGetOption option)
            : base($"Couldn't find handler for {option.GetType().Name}")
        {
        }
    }
}