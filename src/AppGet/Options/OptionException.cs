using AppGet.Exceptions;

namespace AppGet.Options
{
    public abstract class OptionException : AppGetException
    {
        protected OptionException(string message) : base(message)
        {
        }
    }
}