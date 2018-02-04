namespace AppGet.Options
{
    public class UnknownCommandException : OptionException
    {
        public UnknownCommandException(string command)
            : base($"[{command}] is not a valid command.")
        {
        }
    }
}