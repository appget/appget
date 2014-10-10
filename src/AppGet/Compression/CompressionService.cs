using System;
using System.Linq;
using AppGet.ProgressTracker;
using NLog;
using SharpCompress.Archive;
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
                Total = archive.Count()
            };

            foreach (var entry in archive)
            {
                if (!entry.IsDirectory)
                {
                    entry.WriteToDirectory(destination, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                }

                progress.Completed++;

                if (OnStatusUpdates != null)
                {
                    OnStatusUpdates(progress);
                }
                if (OnCompleted != null)
                {
                    OnCompleted(progress);
                }
            }
        }

        public Action<ProgressState> OnStatusUpdates { get; set; }
        public Action<ProgressState> OnCompleted { get; set; }
    }
}
