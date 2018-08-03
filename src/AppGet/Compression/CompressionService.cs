using System;
using System.Linq;
using AppGet.ProgressTracker;
using NLog;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace AppGet.Compression
{
    public interface ICompressionService : IReportProgress
    {
        void Decompress(string sourcePath, string destination);
    }

    public class CompressionService : ICompressionService
    {
        private readonly Logger _logger;

        public CompressionService(Logger logger)
        {
            _logger = logger;
        }

        public void Decompress(string sourcePath, string destination)
        {
            _logger.Info("Extracting package to " + destination);
            var archive = ArchiveFactory.Open(sourcePath).Entries.ToList();

            var progress = new ProgressState
            {
                Total = archive.Count
            };

            foreach (var entry in archive)
            {
                if (!entry.IsDirectory)
                {
                    entry.WriteToDirectory(destination, new ExtractionOptions
                    {
                        ExtractFullPath = true,
                        Overwrite = true,
                        PreserveFileTime = true
                    });
                }

                progress.Completed++;

                OnStatusUpdated?.Invoke(progress);
                OnCompleted?.Invoke(progress);
            }
        }

        public Action<ProgressState> OnStatusUpdated { get; set; }
        public Action<ProgressState> OnCompleted { get; set; }
    }
}