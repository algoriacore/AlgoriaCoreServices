using System;

namespace AlgoriaCore.Application.BaseClases.Dto
{
    public class FileDto
    {
        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FileToken { get; set; }

        public byte[] FileArray { get; set; }

        public string FileBase64 { get; set; }

        public FileDto()
        {
            FileToken = Guid.NewGuid().ToString("N");
        }

        public FileDto(string fileName, string fileType)
        {
            FileName = fileName;
            FileType = fileType;
            FileToken = Guid.NewGuid().ToString("N");
        }
    }
}
