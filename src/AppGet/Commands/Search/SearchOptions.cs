using AppGet.Options;
using CommandLine;

namespace AppGet.Commands.Search
{
    [Verb("search", HelpText = "Search application repository using a term")]
    public class SearchOptions : AppGetOption
    {
        [Value(0, MetaName = "QUERY", HelpText = "Query term to use", Required = true)]
        public string Query { get; set; }
    }
}