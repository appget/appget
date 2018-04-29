using System.Linq;

namespace AppGet.Commands
{
    public abstract class PackageCommandOptions : AppGetOption
    {
        protected const string PACKAGE_META_NAME = "Package";

        public abstract string Package { get; set; }

        public string PackageId => Package.Split(':').FirstOrDefault();

        public string Tag
        {
            get
            {
                var parts = Package.Split(':');

                return parts.Length > 1 ? parts[1] : null;
            }
        }
    }
}