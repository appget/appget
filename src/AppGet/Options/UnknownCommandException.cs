namespace AppGet.Options
{
    public class UnknownCommandException : OptionException
    {
        public UnknownCommandException(string command)
            : base("[{0}] is not a valid command.", command)
        {
        }
    }
}