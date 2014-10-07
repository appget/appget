using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AppGet.Core.ProgressTracker;
using SharpCompress.Archive;
using SharpCompress.Common;
using SharpCompress.Reader;

namespace AppGet.Core.Compression
{
    public class CompressionService : IReportProgress
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
                OnStatusUpdates(progress);
            }
        }

        public Action<ProgressState> OnStatusUpdates { get; set; }
    }
}
