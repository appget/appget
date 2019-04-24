using System;
using System.IO;
using System.Security.Cryptography;

namespace AppGet.Manifest.Hash
{
    public class Sha256 : ICalculateHash
    {
        public string CalculateHash(string file)
        {
            using (var stream = File.OpenRead(file))
            using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(stream);
                return HashToString(hash);
            }
        }

        public string CalculateHash(byte[] buffer)
        {
            using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(buffer);
                return HashToString(hash);
            }
        }


        private static string HashToString(byte[] hash)
        {
            return BitConverter.ToString(hash).ToLowerInvariant().Replace("-", string.Empty);
        }
    }
}