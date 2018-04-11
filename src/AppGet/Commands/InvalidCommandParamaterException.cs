using AppGet.Exceptions;

namespace AppGet.Commands
{
    public class InvalidCommandParamaterException : AppGetException
    {
        public AppGetOption Option { get; }

        public InvalidCommandParamaterException(string message, AppGetOption option)
            : base(message)
        {
            Option = option;
        }
    }
}