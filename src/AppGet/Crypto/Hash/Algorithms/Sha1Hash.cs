using System.Security.Cryptography;
using AppGet.Manifests;

namespace AppGet.Crypto.Hash.Algorithms
{
    public class Sha1Hash : CheckSumBase
    {
        public override HashTypes HashType => HashTypes.Sha1;

        protected override HashAlgorithm GetHashAlgorithm()
        {
            return new SHA1Managed();
        }
    }
}