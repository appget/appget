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
            return string.Format("{0} - v{1}", Id, Version);
        }
    }
}