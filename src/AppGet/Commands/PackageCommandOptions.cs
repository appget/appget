using AppGet.Manifest;
using CommandLine;

namespace AppGet.Commands
{
    public abstract class PackageCommandOptions : AppGetOption
    {
        protected const string PACKAGE_META_NAME = "Package";

        public abstract string Package { get; set; }

        public string PackageId => TagHelper.ParseId(Package);

        public string Tag => TagHelper.ParseTag(Package);


        [Option('r', "repository", HelpText = "Target repository.", Hidden = true)]
        public string Repository { get; set; }
    }
}