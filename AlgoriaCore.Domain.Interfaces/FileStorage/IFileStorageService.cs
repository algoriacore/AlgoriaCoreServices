using System.Threading.Tasks;

namespace AlgoriaCore.Domain.Interfaces.FileStorage
{
    public interface IFileStorageService
    {
        string CreateTempFile(IFileStorageDto request, string path);
        Task<IFileStorageDto> GetTempFileAsync(string tempFileName, string tempPath);
        Task<IFileStorageDto> DownloadTempFile(string filePath, string fileName, string fileType);
    }
}
