using System.Threading.Tasks;
using AppGet.PackageSearch;

namespace AppGet.Commands.Search
{
    public class SearchCommandHandler : ICommandHandler<SearchOptions>
    {
        private readonly IPackageSearchService _packageSearchService;

        public SearchCommandHandler(IPackageSearchService packageSearchService)
        {
            _packageSearchService = packageSearchService;
        }


        public async Task Execute(SearchOptions commandOptions)
        {
            var searchOptions = (SearchOptions)commandOptions;
            await _packageSearchService.DisplayResults(searchOptions.Query);
        }
    }
}