using AppGet.Manifests;

namespace AppGet.Installers.InstallBuilder
{
    public class InstallBuilderDetector : InstallerDetectorBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.InstallBuilder;
        protected override string[] Terms => new[] { "installbuilder", "bitrock" };
    }
}
