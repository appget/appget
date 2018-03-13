using System;

namespace AppGet.Manifests
{
    public class FileVerificationInfo
    {
        public HashTypes HashType { get; set; }
        public string HashValue { get; set; }
        public long FileSize { get; set; }
        public DateTime LastModified { get; set; }
        public string ETag { get; set; }
    }
}