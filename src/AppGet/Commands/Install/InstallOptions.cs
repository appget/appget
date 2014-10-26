using AppGet.Options;
using CommandLine;
using CommandLine.Text;

namespace AppGet.Commands.Install
{
    public class InstallOptions : PackageCommandOptions
    {
        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("<<app title>>", "<<app version>>"),
                Copyright = new CopyrightInfo("<<app author>>", 2014),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("<<license details here.>>");
            help.AddPreOptionsLine("Usage: app -p Someone");
            help.AddOptions(this);
            return help;
        }
    }
}