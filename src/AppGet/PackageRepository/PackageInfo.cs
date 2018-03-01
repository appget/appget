namespace AppGet.PackageRepository
{
    public class PackageInfo
    {
        public string Id { get; set; }
        public string Tag { get; set; }
        public string ManifestUrl { get; set; }

        public override string ToString()
        {
            var text = $"{Id}";
            if (Tag != null)
            {
                text += $":{Tag}";
            }

            return text;
        }
    }
}