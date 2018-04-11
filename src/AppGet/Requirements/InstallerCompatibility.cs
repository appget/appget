using System.Collections.Generic;
using System.Linq;
using AppGet.Manifests;

namespace AppGet.Requirements
{
    public class InstallerCompatibility
    {
        public Installer Installer { get; }
        public List<EnforcementResult> Results { get; }

        public bool IsCompatible => Results.All(r => r.Success);

        public InstallerCompatibility(Installer installer)
        {
            Installer = installer;
            Results = new List<EnforcementResult>();
        }

        public override string ToString()
        {
            if (IsCompatible)
            {
                return $"[OK] {Installer}";
            }

            return $"[Incompatible {Results.Count(r => !r.Success)}]{Installer}";
        }
    }
}