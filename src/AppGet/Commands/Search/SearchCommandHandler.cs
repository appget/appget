using AppGet.Options;
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

        public bool CanExecute(AppGetOption packageCommandOptions)
        {
            return packageCommandOptions is SearchOptions;
        }

        public void Execute(AppGetOption searchCommandOptions)
        {
            var viewOptions = (SearchOptions)searchCommandOptions;
            _packageSearchService.DisplayResults(viewOptions.PackageId);
        }
    }
}