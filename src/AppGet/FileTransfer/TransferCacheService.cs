using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AppGet.FileSystem;
using AppGet.Manifests;
using NLog;

namespace AppGet.FileTransfer
{
    public interface ITransferCacheService
    {
        bool IsValid(string path, FileHash hash);
    }

    public class TransferCacheService : ITransferCacheService
    {
        private readonly IFileSystem _fileSystem;
        private readonly Logger _logger;

        public TransferCacheService(IFileSystem fileSystem, Logger logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public bool IsValid(string path, FileHash hash)
        {
            if (!_fileSystem.FileExists(path))
            {
                return false;
            }

            // TODO: make sure hash matches too!

            _logger.Debug($"Installer is already downloaded: {path}");
            return true;
        }
    }
}
