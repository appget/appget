using System.Security.Cryptography;
using AppGet.Manifests;

namespace AppGet.Crypto.Hash.Algorithms
{
    public class Sha256Hash : CheckSumBase
    {
        public override HashType HashType => HashType.Sha256;

        protected override HashAlgorithm GetHashAlgorithm()
        {
            return new SHA256Managed();
        }
    }
}
