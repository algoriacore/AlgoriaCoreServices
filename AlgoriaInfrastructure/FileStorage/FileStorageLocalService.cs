using AlgoriaCore.Application;
using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.FileStorage;
using AlgoriaCore.Domain.Interfaces.FileStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaInfrastructure.FileStorage
{
    public class FileStorageLocalService : IFileStorageService
    {
        public IAppLocalizationProvider AppLocalizationProvider { get; set; }

        public FileStorageLocalService()
        {

        }
        public string L(string key)
        {
            return AppLocalizationProvider.L(key);
        }

        public string CreateTempFile(IFileStorageDto request, string path)
        {
            var uid = Guid.NewGuid().ToString();
            string tempFileName = "";

            if (request.FileName.IndexOf('.') >= 0)
            {
                tempFileName = string.Format("{0}.{1}", uid, request.FileName.Substring(request.FileName.LastIndexOf('.') + 1));
            }
            else
            {
                tempFileName = uid;
            }

            string tempPath = string.Format("{0}\\{1}", path, tempFileName);

            using (FileStream fs = File.Create(tempPath))
            {
                fs.Write(request.FileArray, 0, request.Length);
            }

            return tempFileName;
        }

        public async Task<IFileStorageDto> GetTempFileAsync(string tempFileName, string tempPath)
        {
            FileStorageDto fResp = null;
            FileInfo fInfo = new FileInfo(tempPath);

            if (fInfo.Exists)
            {
                fResp = new FileStorageDto();
                fResp.FileName = fInfo.Name;
                fResp.Length = (int)fInfo.Length;
                fResp.FileArray = await File.ReadAllBytesAsync(tempPath);
                fResp.ContentType = MimeTypes.GetMimeType(tempPath);
            }

            return fResp;
        }
        public async Task<IFileStorageDto> DownloadTempFile(string filePath, string fileName, string fileType)
        {
            if (!File.Exists(filePath))
            {
                throw new AlgoriaCoreGeneralException(L("RequestedFileDoesNotExists"));
            }

            FileStorageDto fResp = null;
            FileInfo fInfo = new FileInfo(filePath);

            if (fInfo.Exists)
            {
                fResp = new FileStorageDto();
                fResp.FileName = fileName;
                fResp.Length = (int)fInfo.Length;
                fResp.FileArray = File.ReadAllBytes(filePath);
                fResp.ContentType = fileType;

                File.Delete(filePath);
            }

            return fResp;
        }

    }
}
