namespace AppGet.PackageRepository
{
    public class PackageInfo
    {
        public string Id { get; set; }
        public string MajorVersion { get; set; }
        public string ManifestUrl { get; set; }

        public override string ToString()
        {
            var text = $"{Id}";
            if (MajorVersion != null)
            {
                text += $":{MajorVersion}";
            }

            return text;
        }
    }
}