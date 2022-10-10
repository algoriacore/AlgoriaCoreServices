using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.BinaryObjects;
using AlgoriaCore.Application.Managers.Logging;
using AlgoriaCore.Application.QueriesAndCommands.Files._1Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Files._2Queries
{
    public class DownloadTempFileHandler : BaseCoreClass, IRequestHandler<DownloadTempFile, DownloadTempFileResponse>
    {
        private readonly BinaryObjectManager _binaryObject;

        public DownloadTempFileHandler(ICoreServices coreServices, WebLogManager manager, BinaryObjectManager binaryObject) : base(coreServices)
        {
            _binaryObject = binaryObject;
        }

        public async Task<DownloadTempFileResponse> Handle(DownloadTempFile request, CancellationToken cancellationToken)
        {
            var response = await _binaryObject.DownloadTempFile(request);

            return await Task.FromResult(new DownloadTempFileResponse 
            { 
                FileName = response.FileName, 
                Length = response.Length, 
                FileArray = response.FileArray, 
                ContentType = response.ContentType 
            });
        }
    }
}
