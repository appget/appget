using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppGet.Extensions;
using CommandLine;
using CommandLine.Text;

namespace AppGet.Commands
{
    public class AppGetSentenceBuilder : SentenceBuilder
    {
        public override Func<string> RequiredWord
        {
            get { return () => "Required."; }
        }

        public override Func<string> ErrorsHeadingText
        {
            get { return () => ""; }
        }

        public override Func<string> UsageHeadingText
        {
            get { return () => "USAGE:"; }
        }

        public override Func<bool, string> HelpCommandText
        {
            get
            {
                return isOption => isOption
                    ? "Display this help screen."
                    : "Display more information on a specific command.";
            }
        }

        public override Func<bool, string> VersionCommandText
        {
            get { return _ => "Display version information."; }
        }

        public override Func<Error, string> FormatError
        {
            get
            {


                return error =>
                {
                    string GetName(string defaultName = "")
                    {
                        var namedError = (NamedError)error;

                        var name = namedError.NameInfo.NameText;

                        if (name.IsNullOrWhiteSpace())
                        {
                            name = defaultName;
                        }

                        return name;
                    }

                    string GetToken()
                    {
                        var namedError = (TokenError)error;
                        return namedError.Token;
                    }

                    switch (error.Tag)
                    {
                        case ErrorType.BadFormatTokenError:
                            return $"Token '{GetToken()}' is not recognized.";
                        case ErrorType.MissingValueOptionError:
                            return $"Option '{GetName()}' has no value.";
                        case ErrorType.UnknownOptionError:
                            return $"Option '{GetToken()}' is unknown.";
                        case ErrorType.MissingRequiredOptionError:
                            return $"Required option '{GetName("package")}' is missing.";
                        case ErrorType.BadFormatConversionError:
                            return $"Option '{GetName()}' is defined with a bad format.";
                        case ErrorType.SequenceOutOfRangeError:
                            var seqOutRange = ((SequenceOutOfRangeError)error);
                            return seqOutRange.NameInfo.Equals(NameInfo.EmptyName)
                                ? "A sequence value not bound to option name is defined with few items than required."
                                : "A sequence option '".JoinTo(seqOutRange.NameInfo.NameText, "' is defined with fewer or more items than required.");
                        case ErrorType.BadVerbSelectedError:
                            return $"Command '{GetToken()}' is not recognized.";
                        case ErrorType.NoVerbSelectedError:
                            return "No command selected.";
                        case ErrorType.RepeatedOptionError:
                            return $"Option '{GetName()}' is defined multiple times.";
                    }
                    throw new InvalidOperationException();
                };
            }
        }

        public override Func<IEnumerable<MutuallyExclusiveSetError>, string> FormatMutuallyExclusiveSetErrors
        {
            get
            {
                return errors =>
                {
                    var bySet = from e in errors
                                group e by e.SetName into g
                                select new { SetName = g.Key, Errors = g.ToList() };

                    var msgs = bySet.Select(
                        set =>
                        {
                            var names = string.Join(
                                string.Empty,
                                (from e in set.Errors select "'".JoinTo(e.NameInfo.NameText, "', ")).ToArray());
                            var namesCount = set.Errors.Count();

                            var incompat = string.Join(
                                string.Empty,
                                (from x in
                                        (from s in bySet where !s.SetName.Equals(set.SetName) from e in s.Errors select e)
                                        .Distinct()
                                 select "'".JoinTo(x.NameInfo.NameText, "', ")).ToArray());

                            return
                                new StringBuilder("Option")
                                    .AppendWhen(namesCount > 1, "s")
                                    .Append(": ")
                                    .Append(names.Substring(0, names.Length - 2))
                                    .Append(' ')
                                    .AppendIf(namesCount > 1, "are", "is")
                                    .Append(" not compatible with: ")
                                    .Append(incompat.Substring(0, incompat.Length - 2))
                                    .Append('.')
                                    .ToString();
                        }).ToArray();
                    return string.Join(Environment.NewLine, msgs);
                };
            }
        }
    }
}