namespace AppGet.Options
{
    public abstract class PackageCommandOptions : AppGetOption
    {
        protected const string PACKAGE_META_NAME = "PackageID";

        public abstract string PackageId { get; set; }
    }
}