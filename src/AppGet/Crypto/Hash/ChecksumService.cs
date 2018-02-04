using System;
using System.Collections.Generic;
using System.Linq;
using AppGet.Manifests;
using NLog;

namespace AppGet.Crypto.Hash
{
    public interface IChecksumService
    {
        void ValidateHash(string path, FileHash fileHash);
    }

    public class ChecksumService : IChecksumService
    {
        private readonly IEnumerable<ICheckSum> _checkSums;
        private readonly Logger _logger;

        public ChecksumService(IEnumerable<ICheckSum> checkSums, Logger logger)
        {
            _checkSums = checkSums;
            _logger = logger;
        }

        public void ValidateHash(string path, FileHash fileHash)
        {
            _logger.Info("Starting checksum verification");

            var hashAlg = _checkSums.Single(c => c.HashType == fileHash.HashType);

            var hash = hashAlg.GetChecksum(path);

            if (!string.Equals(hash, fileHash.Value, StringComparison.OrdinalIgnoreCase))
            {
                _logger.Warn($"Checksum verification failed for ${path}. File: {hash}  Manifest:{fileHash.Value}");
                throw new ChecksumVerificationException($"${fileHash.HashType.ToString().ToUpperInvariant()} Checksum verification failed for {path}.");
            }

            _logger.Info("Starting checksum verification PASSED");

        }
    }
}
