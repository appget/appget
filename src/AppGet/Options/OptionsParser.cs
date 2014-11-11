using System.Globalization;
using CommandLine;

namespace AppGet.Options
{
    public interface IParseOptions
    {
        AppGetOption Parse(params string[] args);
    }

    public class OptionsParser : IParseOptions
    {
        public AppGetOption Parse(params string[] args)
        {
            var parser = new Parser(Configure);

            var rootOptions = new RootOptions();

            AppGetOption appGetOption = null;

            var c = parser.ParseArguments(args, rootOptions, (commandName, parsedOption) =>
            {
                appGetOption = (AppGetOption)parsedOption;

                if (appGetOption == null)
                {
                    throw new UnknownCommandException(commandName);
                }

                appGetOption.RootOptions = rootOptions;
                appGetOption.ProcessArgs();


                appGetOption.CommandName = commandName;
            });

            return appGetOption;
        }

        private void Configure(ParserSettings settings)
        {
            settings.CaseSensitive = false;
            settings.IgnoreUnknownArguments = true;
            settings.ParsingCulture = CultureInfo.InvariantCulture;
        }


    }
}