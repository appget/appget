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
            using (var algorithm = new SHA256Managed())
            {
                var checksum = algorithm.ComputeHash(stream);
                return BitConverter.ToString(checksum).ToLowerInvariant().Replace("-", string.Empty);
            }
        }

        public string CalculateHash(byte[] buffer)
        {
            using (var algorithm = new SHA256Managed())
            {
                var checksum = algorithm.ComputeHash(buffer);
                return BitConverter.ToString(checksum).ToLowerInvariant().Replace("-", string.Empty);
            }
        }
    }
}