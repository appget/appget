using System;
using System.Collections.Generic;
using System.Linq;
using AppGet.Manifests;
using NLog;

namespace AppGet.Crypto.Hash
{
    public interface IChecksumService
    {
        void ValidateHash(string path, FileVerificationInfo fileVerificationInfo);
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

        public void ValidateHash(string path, FileVerificationInfo fileVerificationInfo)
        {
            _logger.Trace("Starting checksum verification");

            var hashAlg = _checkSums.Single(c => c.HashType == fileVerificationInfo.HashType);

            var hash = hashAlg.CalculateHash(path);

            if (!string.Equals(hash, fileVerificationInfo.HashValue, StringComparison.OrdinalIgnoreCase))
            {
                _logger.Warn($"Checksum verification failed for {path}. File: {hash}  Manifest:{fileVerificationInfo.HashValue}");
                throw new ChecksumVerificationException($"{fileVerificationInfo.HashType.ToString().ToUpperInvariant()} Checksum verification failed for {path}.");
            }

            _logger.Info("Checksum verification PASSED");

        }
    }
}
