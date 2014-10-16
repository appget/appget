using System.Collections.Generic;
using System.Linq;
using AppGet.FlightPlans;

namespace AppGet.Requirements
{
    public class InstallerDecision
    {
        public Installer Installer { get; set; }
        public List<EnforcementResult> Results { get; set; }

        public bool Approved
        {
            get
            {
                return Results.All(r => r.Success);
            }
        }

        public InstallerDecision(Installer installer)
        {
            Installer = installer;
            Results = new List<EnforcementResult>();
        }

        public override string ToString()
        {
            if (Approved)
            {
                return "[OK] " + Installer;
            }

            return "[Rejected " + Results.Count(r => !r.Success) + "]" + Installer;
        }
    }
}
