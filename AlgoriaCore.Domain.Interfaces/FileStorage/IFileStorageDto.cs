using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Domain.Interfaces.FileStorage
{
    public interface IFileStorageDto
    {
        string Name { get; set; }
        string FileName { get; set; }
        int Length { get; set; }
        byte[] FileArray { get; set; }
        string ContentDisposition { get; set; }
        string ContentType { get; set; }
    }
}
