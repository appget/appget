using AppGet.CreatePackage;
using AppGet.Manifests;
using SevenZip;

namespace AppGet.Installers
{
    public interface IDetectInstallMethod
    {
        InstallMethodTypes InstallMethod { get; }
        Confidence GetConfidence(string path, SevenZipExtractor archive, string exeManifest);
    }
}