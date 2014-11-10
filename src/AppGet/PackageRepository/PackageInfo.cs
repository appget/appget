namespace AppGet.PackageRepository
{
    public class PackageInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string MajorVersion { get; set; }
        public string FlightPlanUrl { get; set; }
        public string SourceUrl { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Version))
            {
                return Id;
            }

            var result = string.Format("[{0}] {1}", Id, Name);

            if (!string.IsNullOrEmpty(Version))
            {
                result += " (" + Version + ")";
            }

            return result;
        }
    }
}