using AlgoriaCore.Domain.Interfaces.Excel;
using System;

namespace AlgoriaCore.Domain.Excel
{
    public class FileExcel : IFileExcel
    {
        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FileToken { get; set; }

        public byte[] FileArray { get; set; }

        public FileExcel(string fileName, string fileType)
        {
            FileName = fileName;
            FileType = fileType;
            FileToken = Guid.NewGuid().ToString("N");
        }
    }
}
