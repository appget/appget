using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppGet.Manifests;

namespace AppGet.Crypto.Hash
{
    public interface IChecksumService
    {
        void ValidateHash(string path, FileHash fileHash);
    }

    public class ChecksumService : IChecksumService
    {
        private readonly IEnumerable<ICheckSum> _checkSums;

        public ChecksumService(IEnumerable<ICheckSum> checkSums)
        {
            _checkSums = checkSums;
        }

        public void ValidateHash(string path, FileHash fileHash)
        {
            var hashAlg = _checkSums.Single(c => c.HashType == fileHash.HashType);

            var hash = hashAlg.GetChecksum(path);

            if (!string.Equals(hash, fileHash.Value, StringComparison.OrdinalIgnoreCase))
            {
                throw new ChecksumVerificationException($"${fileHash.HashType.ToString().ToUpperInvariant()} Checksum verification failed for {path}.");
            }
        }
    }
}
