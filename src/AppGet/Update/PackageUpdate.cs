namespace AppGet.Update
{
    public enum UpdateStatus
    {
        Unknown,
        UpToDate,
        Available,
        UnknownNewerVersion,
        UnComparable
    }

    public class PackageUpdate
    {
        public string InstallerId { get; set; }
        public string PackageId { get; set; }
        public string InstalledVersion { get; set; }
        public string AvailableVersion { get; set; }
        public UpdateStatus Status { get; set; }
    }
}