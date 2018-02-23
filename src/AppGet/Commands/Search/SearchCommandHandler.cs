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

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is SearchOptions;
        }

        public Task Execute(AppGetOption commandOptions)
        {
            var searchOptions = (SearchOptions)commandOptions;
            _packageSearchService.DisplayResults(searchOptions.Query);

            return Task.FromResult(0);
        }
    }
}