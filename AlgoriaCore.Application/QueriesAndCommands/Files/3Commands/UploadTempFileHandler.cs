using AlgoriaCore.Application.Managers.BinaryObjects;
using AlgoriaCore.Application.QueriesAndCommands.Files._1Model;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Files._3Commands
{
    public class UploadTempFileHandler : IRequestHandler<UploadTempFile, FileUploadResponse>
    {
        private readonly BinaryObjectManager _binaryObject;

        public UploadTempFileHandler(BinaryObjectManager binaryObject,
            ILogger<UploadTempFileHandler> logger)
        {
            _binaryObject = binaryObject;
        }

        public async Task<FileUploadResponse> Handle(UploadTempFile request, CancellationToken cancellationToken)
        {
            var fu = _binaryObject.CreateTempFile(new Managers.BinaryObjects.Dto.FileDto
            {
                ContentDisposition = request.ContentDisposition,
                ContentType = request.ContentType,
                FileArray = request.FileArray,
                FileName = request.FileName,
                Length = request.Length,
                Name = request.Name
            });

            return await Task.FromResult(new FileUploadResponse
            {
                FileName = fu.FileName,
                FileSize = fu.Length,
                TempFileName = fu.FileName
            });
        }
    }
}
