using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace AppGet.Crypto.Certificates
{
    public class FileSignatureService
    {
        public X509Certificate Get(string filePath)
        {
            try
            {
                var file = FileVersionInfo.GetVersionInfo(filePath);
                var c = X509Certificate.CreateFromSignedFile(filePath);
                return c;
            }
            catch (CryptographicException)
            {
                return null;
            }
        }
    }
}
