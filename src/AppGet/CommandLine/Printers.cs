using System;
using System.Collections.Generic;
using System.Linq;
using AppGet.Extensions;
using AppGet.PackageRepository;

namespace AppGet.CommandLine
{
    public static class Printers
    {
        public static void Print(List<PackageInfo> packages)
        {
            var idColumnWidth = packages.Max(c => c.Id.Length) + 1;
            var nameColumnWidth = packages.Max(c => c.Name?.Length ?? 6) + 1;
            var tagColumnWidth = packages.Max(c => c.Tag.Length) + 2;
            var versionColumnWidth = packages.Max(c => c.Version?.Length ?? 6) + 3;

            var header = $"{"ID".PadRight(idColumnWidth)} {"Name".PadRight(nameColumnWidth)} {"Version".PadRight(versionColumnWidth)} {"Tag".PadRight(tagColumnWidth)}";
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));


            foreach (var package in packages)
            {
                var id = package.Id.PadRight(idColumnWidth);
                var name = package.Name.OrBlank().PadRight(nameColumnWidth);
                var version = package.Version.OrBlank().PadRight(versionColumnWidth);
                var tag = package.Tag.OrBlank().PadRight(tagColumnWidth);


                Console.WriteLine($"{id} {name} {version} {tag}");
            }
        }

    }
}
