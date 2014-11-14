using System.Collections.Generic;
using System.Linq;
using AppGet.Requirements;

namespace AppGet.Manifests
{
    public interface IFindInstaller
    {
        Installer GetBestInstaller(List<Installer> installers);
        List<InstallerCompatibility> ProcessRequirements(List<Installer> installers);
    }

    public class FindInstaller : IFindInstaller
    {
        private readonly IEnumerable<IEnforceRequirements> _requirements;

        public FindInstaller(IEnumerable<IEnforceRequirements> requirements)
        {
            _requirements = requirements;
        }

        //TODO: We should return the package the user specified, if they supplied one
        public Installer GetBestInstaller(List<Installer> installers)
        {
            var decisions = ProcessRequirements(installers);
            var compatible = decisions.Where(d => d.IsCompatible).ToList();

            if (!compatible.Any()) return null;

            return compatible.OrderByDescending(d => d.Installer.Architecture).First().Installer;
        }

        public List<InstallerCompatibility> ProcessRequirements(List<Installer> installers)
        {
            var decisions = new List<InstallerCompatibility>();

            foreach (var installer in installers)
            {
                var decision = new InstallerCompatibility(installer);

                foreach (var requirement in _requirements)
                {
                    decision.Results.Add(requirement.IsRequirementSatisfied(installer));
                }

                decisions.Add(decision);
            }

            return decisions;
        }
    }
}
