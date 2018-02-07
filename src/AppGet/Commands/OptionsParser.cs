using AppGet.Commands.Install;
using AppGet.Commands.List;
using AppGet.Commands.Search;
using AppGet.Commands.Uninstall;
using AppGet.Commands.ViewManifest;
using CommandLine;

namespace AppGet.Commands
{
    public interface IParseOptions
    {
        AppGetOption Parse(params string[] args);
    }

    public class OptionsParser : IParseOptions
    {
        public AppGetOption Parse(params string[] args)
        {
            var result = Parser.Default.ParseArguments<
                InstallOptions,
                ListOptions,
                SearchOptions,
                UninstallOptions,
                ViewManifestOptions>
                (args);

            if (result.Tag == ParserResultType.Parsed)
            {
                return (AppGetOption)((Parsed<object>)result).Value;
            }

            return null;
        }

    }
}