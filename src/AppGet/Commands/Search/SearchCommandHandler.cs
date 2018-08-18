using System.Threading.Tasks;
using AppGet.PackageSearch;

namespace AppGet.Commands.Search
{
    public class SearchCommandHandler : ICommandHandler
    {
        private readonly IPackageSearchService _packageSearchService;

        public SearchCommandHandler(IPackageSearchService packageSearchService)
        {
            _packageSearchService = packageSearchService;
        }


        public async Task Execute(AppGetOption commandOptions)
        {
            var searchOptions = (SearchOptions)commandOptions;
            await _packageSearchService.DisplayResults(searchOptions.Query);
        }
    }
}