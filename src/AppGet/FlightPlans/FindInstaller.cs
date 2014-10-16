using System.Collections.Generic;
using System.Linq;
using AppGet.FlightPlans;
using AppGet.Requirements;

namespace AppGet.Packages
{
    public interface IFindInstaller
    {
        Installer GetBestInstaller(List<Installer> installers);
        List<InstallerDecision> ProcessRequirements(List<Installer> installers);
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
            var approved = decisions.Where(d => d.Approved).ToList();

            if (!approved.Any()) return null;

            return approved.OrderByDescending(d => d.Installer.Architecture).First().Installer;
        }

        public List<InstallerDecision> ProcessRequirements(List<Installer> installers)
        {
            var decisions = new List<InstallerDecision>();

            foreach (var installer in installers)
            {
                var decision = new InstallerDecision(installer);

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
