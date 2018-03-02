using AppGet.CreatePackage;
using AppGet.Manifests;
using SevenZip;

namespace AppGet.Installers.Squirrel
{
    public class SquirrelDetector : InstallerDetectorBase
    {
        private readonly ISquirrelReader _squirrelReader;
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.Squirrel;


        public SquirrelDetector(ISquirrelReader squirrelReader)
        {
            _squirrelReader = squirrelReader;
        }

        public override Confidence GetConfidence(string path, SevenZipExtractor archive, string exeManifest)
        {
            var baseConfidence = base.GetConfidence(path, archive, exeManifest);
            if (baseConfidence > Confidence.None)
            {
                return baseConfidence;
            }

            if (archive != null)
            {
                return _squirrelReader.IsSquirrel(archive) ? Confidence.Authoritative : Confidence.None;
            }

            return Confidence.None;
        }
    }
}
