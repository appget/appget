using System.Security.Cryptography;
using AppGet.Manifest;
using AppGet.Manifests;

namespace AppGet.Crypto.Hash.Algorithms
{
    public class Md5Hash : CheckSumBase
    {
        public override HashTypes HashType => HashTypes.Md5;

        protected override HashAlgorithm GetHashAlgorithm()
        {
            return MD5.Create();
        }
    }
}