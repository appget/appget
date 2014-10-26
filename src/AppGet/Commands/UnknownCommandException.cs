using AppGet.Exceptions;

namespace AppGet.Commands
{
    public class UnknownCommandException : AppGetException
    {
        public UnknownCommandException(string commandName)
            : base("Unknown command name [{0}]", commandName)
        {

        }
    }
}