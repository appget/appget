using System;
using System.Globalization;
using System.Linq;
using CommandLine;

namespace AppGet.Options
{
    public interface IOptionsService
    {
        CommandOptions Parse(params string[] args);
    }

    public class OptionsService : IOptionsService
    {
        public CommandOptions Parse(params string[] args)
        {

            var parser = new Parser(Configure);

            var rootOptions = new RootOptions();

            CommandOptions commandOptions = null;

            var c = parser.ParseArguments(args, rootOptions, (commandName, parsedOption) =>
            {
                commandOptions = (CommandOptions)parsedOption;

                if (commandOptions == null)
                {
                    throw new UnknownCommandException(commandName);
                }

                commandOptions.ProcessUnknowArgs();

                if (commandOptions.UnknowArgs.Any())
                {
                    throw new UnknownOptionException(commandOptions.UnknowArgs);
                }

                commandOptions.CommandName = commandName;
            });

            return commandOptions;
        }

        private void Configure(ParserSettings settings)
        {
            settings.CaseSensitive = false;
            settings.IgnoreUnknownArguments = true;
            settings.ParsingCulture = CultureInfo.InvariantCulture;
        }

        private void OnVerbCommand(string arg1, object arg2)
        {
            throw new NotImplementedException();
        }
    }
}