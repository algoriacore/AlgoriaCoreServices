using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Configuration
{
    public class FileStorageOptions
    {
        public FileStorageMethod StorageMethod { get; set; }
        public FileStorageS3 S3 { get; set; }
        public CloudWatchLogs CloudWatch { get; set; }
    }

    public enum FileStorageMethod : byte
    {
        Normal = 0,
        S3 = 1
    }

    public class FileStorageS3
    {
        public string Id { get; set; }
        public string SecretKey { get; set; }
        public string Bucket { get; set; }
    }

    public class CloudWatchLogs
    {
        public string Id { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
        public string LogGroup { get; set; }
    }
}
