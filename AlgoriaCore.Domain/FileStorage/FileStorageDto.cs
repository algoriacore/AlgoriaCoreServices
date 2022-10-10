using AlgoriaCore.Domain.Interfaces.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Domain.FileStorage
{
    public class FileStorageDto : IFileStorageDto
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public int Length { get; set; }
        public byte[] FileArray { get; set; }
        public string ContentDisposition { get; set; }
        public string ContentType { get; set; }
    }
}
