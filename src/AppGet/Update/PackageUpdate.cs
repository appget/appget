namespace AppGet.Update
{
    public enum UpdateStatus
    {
        UpToDate,
        Available
    }

    public class PackageUpdate
    {
        public string PackageId { get; set; }
        public string Name { get; set; }
        public string InstalledVersion { get; set; }
        public string AvailableVersion { get; set; }
        public string InstallationPath { get; set; }
        public UpdateStatus Status { get; set; }
    }
}