using SevenZip;

namespace AppGet.Installers
{
    public interface IDetectInstallMethod 
    {
        decimal GetConfidence(string path, SevenZipExtractor zip);
    }
}