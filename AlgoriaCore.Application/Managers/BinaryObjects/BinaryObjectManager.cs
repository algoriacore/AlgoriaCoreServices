using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.BinaryObjects.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Files._2Queries;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.FileStorage;
using AlgoriaCore.Domain.Interfaces.FileStorage;
using AlgoriaCore.Domain.Interfaces.Folder;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.BinaryObjects
{
    public class BinaryObjectManager : BaseManager
    {
        private readonly IRepository<AlgoriaCore.Domain.Entities.BinaryObjects, Guid> _repository;
        private readonly IAppFolders _appFolders;
        private readonly IFileStorageService _fileStorageService;

        public BinaryObjectManager(IRepository<AlgoriaCore.Domain.Entities.BinaryObjects, Guid> repository,
                        IConfiguration configuration,
                        IAppFolders appFolders,
                        IFileStorageService fileStorageService)
        {
            _repository = repository;
            _appFolders = appFolders;
            _fileStorageService = fileStorageService;
        }
        public async Task<Guid> CreateAsync(byte[] file)
        {
            var b = new AlgoriaCore.Domain.Entities.BinaryObjects();
            b.Id = Guid.NewGuid();
            b.Bytes = file;

            await _repository.InsertAsync(b);

            return b.Id;
        }
        public async Task<Guid> CreateAsync(byte[] file, string fileName)
        {
            var b = new AlgoriaCore.Domain.Entities.BinaryObjects();
            b.Id = Guid.NewGuid();
            b.Bytes = file;
            b.FileName = fileName;

            await _repository.InsertAsync(b);

            return b.Id;
        }

        public async Task UpdateAsync(Guid id, byte[] file)
        {
            var entity = await _repository.FirstOrDefaultAsync(id);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("BinaryObject"), id));
            }

            entity.Bytes = file;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<byte[]> GetFileAsync(Guid id)
        {
            byte[] b = null;
            var f = await _repository.FirstOrDefaultAsync(m => m.Id == id);

            if (f != null)
            {
                b = f.Bytes;
            }

            return b;
        }

        public async Task<string> GetFileNameAsync(Guid id)
        {
            string fileName = null;
            var f = await _repository.FirstOrDefaultAsync(m => m.Id == id);

            if (f != null)
            {
                fileName = f.FileName;
            }

            return fileName;
        }


        public string getTempPath()
        {
            return _appFolders.TempFileDownloadFolder;
        }

        public FileDto CreateTempFile(FileDto request)
        {

            var path = getTempPath();

            FileStorageDto sentFile = new FileStorageDto();

            sentFile.Name = request.Name;
            sentFile.FileName = request.FileName;
            sentFile.Length = request.Length;
            sentFile.FileArray = request.FileArray;
            sentFile.ContentDisposition = request.ContentDisposition;
            sentFile.ContentType = request.ContentType;

            var tempFileName = _fileStorageService.CreateTempFile(sentFile, path);

            return new FileDto
            {
                FileName = tempFileName,
                Length = request.Length,
                Name = request.Name
            };

        }

        public async Task<FileDto> GetTempFileAsync(string fileName)
        {
            var path = getTempPath();
            string tempPath = string.Format("{0}\\{1}", path, fileName);
            FileDto fResp = null;

            var fileStorage = await _fileStorageService.GetTempFileAsync(fileName, tempPath);

            fResp = new FileDto();
            fResp.FileName = fileStorage.FileName;
            fResp.Length = fileStorage.Length;
            fResp.FileArray = fileStorage.FileArray;
            fResp.ContentType = fileStorage.ContentType;

            return fResp;
        }

        public async Task<FileDto> DownloadTempFile(DownloadTempFile tempFile)
        {
            var filePath = Path.Combine(_appFolders.TempFileDownloadFolder, tempFile.FileToken);
            FileDto fResp = null;

            var fileStorage = await _fileStorageService.DownloadTempFile(filePath, tempFile.FileName, tempFile.FileType);

            fResp = new FileDto();
            fResp.FileName = fileStorage.FileName;
            fResp.Length = fileStorage.Length;
            fResp.FileArray = fileStorage.FileArray;
            fResp.ContentType = fileStorage.ContentType;

            return fResp;
        }
    }
}
