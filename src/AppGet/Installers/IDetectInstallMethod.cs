using AppGet.Manifests;
using SevenZip;

namespace AppGet.Installers
{
    public interface IDetectInstallMethod
    {
        InstallMethodTypes InstallMethod { get; }
        decimal GetConfidence(string path, SevenZipExtractor archive, string exeManifest);
    }
}