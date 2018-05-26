using System;
using System.Collections.Generic;
using System.Linq;
using AppGet.Manifest;
using AppGet.Manifests;
using NLog;

namespace AppGet.Crypto.Hash
{
    public interface IChecksumService
    {
        void ValidateHash(string path, string sha256);
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

        public void ValidateHash(string path, string sha256)
        {
            _logger.Trace("Starting checksum verification");

            var hashAlg = _checkSums.Single(c => c.HashType == HashTypes.Sha256);

            var hash = hashAlg.CalculateHash(path);

            if (!string.Equals(hash, sha256, StringComparison.OrdinalIgnoreCase))
            {
                _logger.Warn($"Checksum verification failed for {path}. File: {hash}  Manifest:{sha256}");

                throw new ChecksumVerificationException(
                    $"SHA256 Checksum verification failed for {path}.");
            }

            _logger.Info("Checksum verification PASSED");
        }
    }
}