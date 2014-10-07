using AppGet.Exceptions;

namespace AppGet.Commands
{
    public class UnknownCommandException : AppGetException
    {
        public UnknownCommandException(string commandName)
            : base("Unknow command name " + commandName)
        {

        }
    }
}