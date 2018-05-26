using System;
using AppGet.Manifest.Hash;
using NLog;

namespace AppGet.Crypto.Hash
{
    public interface IChecksumService
    {
        void ValidateHash(string path, string sha256);
    }

    public class ChecksumService : IChecksumService
    {
        private readonly ICalculateHash _calculateHash;
        private readonly Logger _logger;

        public ChecksumService(ICalculateHash calculateHash, Logger logger)
        {
            _calculateHash = calculateHash;
            _logger = logger;
        }

        public void ValidateHash(string path, string sha256)
        {
            _logger.Trace("Starting checksum verification");


            var hash = _calculateHash.CalculateHash(path);

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