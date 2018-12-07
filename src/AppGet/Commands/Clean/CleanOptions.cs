using CommandLine;

namespace AppGet.Commands.Clean
{
    [Verb("clean", HelpText = "Clean AppGet's Installer cache")]
    public class CleanOptions : AppGetOption
    {
    }
}