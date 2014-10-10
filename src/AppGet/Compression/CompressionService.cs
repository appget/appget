using System;
using System.Linq;
using AppGet.ProgressTracker;
using SharpCompress.Archive;
using SharpCompress.Common;

namespace AppGet.Compression
{
    public interface ICompressionService : IReportProgress
    {
        void Decompress(string sourcePath, string sourceDestination);
    }

    public class CompressionService : ICompressionService
    {
        public void Decompress(string sourcePath, string sourceDestination)
        {
            var archive = ArchiveFactory.Open(sourcePath).Entries.ToList();

            var progress = new ProgressState
            {
                Total = archive.Count()
            };

            foreach (var entry in archive)
            {
                if (!entry.IsDirectory)
                {
                    Console.WriteLine(entry.FilePath);
                    entry.WriteToDirectory(sourceDestination, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
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
