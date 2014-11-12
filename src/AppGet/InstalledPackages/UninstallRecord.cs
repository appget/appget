using AppGet.FlightPlans;

namespace AppGet.InstalledPackages
{
    public class UninstallRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string UninstallCommand { get; set; }
        public string InstallLocation { get; set; }
        public string Publisher { get; set; }
        public InstallMethodType InstallMethod { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as UninstallRecord;

            if (other == null)
            {
                return false;
            }

            return other.Id == Id;
        }
    }
}
