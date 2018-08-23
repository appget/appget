using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppGet.Commands.CreateManifest;
using AppGet.Commands.Install;
using AppGet.Commands.Outdated;
using AppGet.Commands.Search;
using AppGet.Commands.Uninstall;
using AppGet.Commands.Update;
using AppGet.Commands.ViewManifest;
using CommandLine;
using CommandLine.Text;
using JetBrains.Annotations;

namespace AppGet.Commands
{
    public interface IParseOptions
    {
        AppGetOption Parse(params string[] args);
    }

    [UsedImplicitly]
    public class OptionsParser : IParseOptions
    {
        public OptionsParser(AppGetSentenceBuilder sentenceBuilder)
        {
            SentenceBuilder.Factory = () => sentenceBuilder;
        }

        public List<string> ParseUrl(string url)
        {
            var uri = new Uri(url);
            var queryString = HttpUtility.ParseQueryString(uri.Query);

            var args = queryString.AllKeys.ToDictionary(k => k, k => queryString[k]).Select(c => $"-{c.Key} {c.Value}").ToList();

            args.InsertRange(0, uri.Segments.Where(c => c != "/"));
            args.Insert(0, uri.Host);

            return args;
        }


        public AppGetOption Parse(params string[] args)
        {
            var cleanArgs = args.Select(c =>
            {
                if (c.StartsWith("/"))
                {
                    return "-" + c.Remove(0, 1);
                }

                return c;
            }).ToList();

            if (cleanArgs.Count() == 1 && cleanArgs[0].StartsWith("appget://"))
            {
                cleanArgs = ParseUrl(cleanArgs.Single());
            }

            var parser = new Parser(c =>
            {
                c.CaseSensitive = true;
                c.IgnoreUnknownArguments = false;
                c.HelpWriter = new CustomHelpWriter();
            });

            var result = parser.ParseArguments<
                InstallOptions,
                SearchOptions,
                UninstallOptions,
                CreateManifestOptions,
                ViewManifestOptions,
                OutdatedOptions,
                UpdateOptions>(cleanArgs);


            switch (result.Tag)
            {
                case ParserResultType.Parsed:
                    {
                        return (AppGetOption)((Parsed<object>)result).Value;
                    }
                case ParserResultType.NotParsed:
                    {
                        var notParsed = (NotParsed<object>)result;
                        throw new CommandLineParserException(notParsed.Errors);
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(result.Tag), result.Tag, "Invalid Tag");
                    }
            }
        }
    }

    public class CommandLineParserException : Exception
    {
        public List<Error> ParseErrors { get; }

        public CommandLineParserException(IEnumerable<Error> parseErrors)
        {
            ParseErrors = parseErrors.ToList();
        }
    }
}