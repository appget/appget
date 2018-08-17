using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AppGet.Commands.CreateManifest;
using AppGet.Commands.Install;
using AppGet.Commands.Outdated;
using AppGet.Commands.Search;
using AppGet.Commands.Uninstall;
using AppGet.Commands.Update;
using AppGet.Commands.ViewManifest;
using AppGet.Extensions;
using CommandLine;
using JetBrains.Annotations;
using Console = Colorful.Console;

namespace AppGet.Commands
{
    public class CustomHelpWriter : TextWriter
    {
        public override Encoding Encoding { get; }

        public override void Write(string value)
        {
            var lines = Regex.Split(value, "\r\n|\r|\n");

            if (lines.Length < 3)
            {
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }

                return;
            }

            foreach (var line in lines)
            {
                if (line.StartsWith("Copyright")) continue;
                if (line.StartsWith("AppGet ")) continue;

                if (line.StartsWith("ERROR"))
                {
                    Console.WriteLine("");
                    continue;
                }

                if (line.StartsWith("  ") || line.IsNullOrWhiteSpace())
                {
                    Console.WriteLine(line);
                    continue;
                }

                Console.WriteLine(line, Color.Red);
            }
        }
    }

    public interface IParseOptions
    {
        AppGetOption Parse(params string[] args);
    }

    [UsedImplicitly]
    public class OptionsParser : IParseOptions
    {
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

            if (result.Tag == ParserResultType.Parsed)
            {
                return (AppGetOption)((Parsed<object>)result).Value;
            }

            return null;
        }
    }
}