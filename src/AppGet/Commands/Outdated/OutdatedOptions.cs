using CommandLine;

namespace AppGet.Commands.Outdated
{
    [Verb("outdated", HelpText = "Check for available updates")]
    public class OutdatedOptions : AppGetOption
    {
    }
}