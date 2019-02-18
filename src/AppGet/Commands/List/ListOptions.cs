using CommandLine;

namespace AppGet.Commands.List
{
    [Verb("list", HelpText = "List all currently installed packages known to AppGet")]
    public class ListOptions : AppGetOption
    {
    }
}