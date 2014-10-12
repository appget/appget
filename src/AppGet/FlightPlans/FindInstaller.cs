using System;
using System.Collections.Generic;
using System.Linq;
using AppGet.FlightPlans;
using AppGet.HostSystem;
using NLog;

namespace AppGet.Packages
{
    public interface IFindInstaller
    {
        Installer GetPackage(List<Installer> packages);
    }

    public class FindInstaller : IFindInstaller
    {
        private readonly IEnvironmentProxy _environmentProxy;

        public FindInstaller(IEnvironmentProxy environmentProxy)
        {
            _environmentProxy = environmentProxy;
        }

        //TODO: We should return the package the user specified, if they supplied one
        public Installer GetPackage(List<Installer> packages)
        {
            //check if x64 and prefer x64, then
            //TODO: what if there is more than one package based on architecture?

            if (_environmentProxy.Is64BitOperatingSystem)
            {
                var x64Package = packages.FirstOrDefault(p => p.Architecture == ArchitectureType.x64);

                if (x64Package != null)
                {
                    return x64Package;
                }
            }

            var x86Package = packages.FirstOrDefault(p => p.Architecture == ArchitectureType.x86);

            if (x86Package != null)
            {
                return x86Package;
            }

            var anyPackage = packages.FirstOrDefault(p => p.Architecture == ArchitectureType.Any);

            if (anyPackage != null)
            {
                return anyPackage;
            }

            throw new NotSupportedException("Unable to find an acceptable package");
        }
    }
}
