using System;
using System.IO;
using System.Linq;
using AppGet.ProgressTracker;
using NLog;
using SevenZip;
using SharpCompress.Archives;
using SharpCompress.Readers;

namespace AppGet.Compression
{
    public interface ICompressionService : IReportProgress
    {
        void Decompress(string sourcePath, string destination);
        SevenZipExtractor TryOpen(string path);
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
                    entry.WriteToDirectory(destination, new ExtractionOptions { ExtractFullPath = true, Overwrite = true, PreserveFileTime = true });
                }

                progress.Completed++;

                OnStatusUpdated?.Invoke(progress);
                OnCompleted?.Invoke(progress);
            }
        }

        public SevenZipExtractor TryOpen(string path)
        {
            foreach (var format in Enum.GetValues(typeof(InArchiveFormat)).Cast<InArchiveFormat>())
            {
                try
                {
                    var archive = new SevenZipExtractor(path, format);
                    if (archive.FilesCount != 0)
                    {
                        return archive;
                    }
                }
                catch (SevenZipArchiveException)
                {

                }
            }

            return null;
        }

        public Action<ProgressState> OnStatusUpdated { get; set; }
        public Action<ProgressState> OnCompleted { get; set; }
    }
}
