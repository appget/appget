using System;
using System.Threading.Tasks;
using AppGet.PackageRepository;
using NLog;

namespace AppGet.PackageSearch
{
    public interface IPackageSearchService
    {
        Task DisplayResults(string query);
    }

    public class PackageSearchService : IPackageSearchService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly Logger _logger;

        public PackageSearchService(IPackageRepository packageRepository, Logger logger)
        {
            _packageRepository = packageRepository;
            _logger = logger;
        }

        public async Task DisplayResults(string query)
        {
            var results = await _packageRepository.Search(query);

            _logger.Info("Found {0} package(s)", results.Count);
            Console.WriteLine();

            foreach (var package in results)
            {
                Console.WriteLine($" {package}");
            }
        }
    }
}