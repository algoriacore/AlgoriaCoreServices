using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Logging;
using AlgoriaCore.Application.QueriesAndCommands.Logging._1Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Logging._2Queries
{
    public class WebLogDownloadZipQueryHandler : BaseCoreClass, IRequestHandler<WebLogDownloadZipQuery, WebLogDownloadZipResponse>
    {
        private readonly WebLogManager _manager;

        public WebLogDownloadZipQueryHandler(ICoreServices coreServices, WebLogManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<WebLogDownloadZipResponse> Handle(WebLogDownloadZipQuery request, CancellationToken cancellationToken)
        {
            var response = _manager.DownloadWebLogs();

            return await Task.FromResult(new WebLogDownloadZipResponse 
            { 
                FileName = response.FileName, 
                FileToken = response.FileToken, 
                FileType = response.FileType 
            });
        }
    }
}
