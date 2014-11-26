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

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is SearchOptions;
        }

        public void Execute(AppGetOption commandOptions)
        {
            var viewOptions = (SearchOptions)commandOptions;
            _packageSearchService.DisplayResults(viewOptions.PackageId);
        }
    }
}