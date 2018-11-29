using System;
using System.Collections.Generic;

namespace AppGet.Installers.Requirements
{
    public class PackageNotCompatibleException : ApplicationException
    {
        public IEnumerable<InstallerCompatibility> Results { get; }

        public PackageNotCompatibleException(IEnumerable<InstallerCompatibility> results)
            : base("None of the available installers are compatible with your system.")
        {
            Results = results;
        }
    }
}