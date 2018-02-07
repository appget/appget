using AppGet.Exceptions;

namespace AppGet.Commands
{
    public abstract class OptionException : AppGetException
    {
        protected OptionException(string message) : base(message)
        {
        }
    }
}