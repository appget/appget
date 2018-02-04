using System.Security.Cryptography;
using AppGet.Manifests;

namespace AppGet.Crypto.Hash.Algorithms
{
    public class Sha1Hash : CheckSumBase
    {
        public override HashType HashType => HashType.Sha1;

        protected override HashAlgorithm GetHashAlgorithm()
        {
            return new SHA1Managed();
        }
    }

}