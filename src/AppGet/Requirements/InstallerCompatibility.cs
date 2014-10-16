using System.Collections.Generic;
using System.Linq;
using AppGet.FlightPlans;

namespace AppGet.Requirements
{
    public class InstallerCompatibility
    {
        public Installer Installer { get; set; }
        public List<EnforcementResult> Results { get; set; }

        public bool Compatible
        {
            get
            {
                return Results.All(r => r.Success);
            }
        }

        public InstallerCompatibility(Installer installer)
        {
            Installer = installer;
            Results = new List<EnforcementResult>();
        }

        public override string ToString()
        {
            if (Compatible)
            {
                return "[OK] " + Installer;
            }

            return "[Incompatible " + Results.Count(r => !r.Success) + "]" + Installer;
        }
    }
}
