using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Logging;
using AlgoriaCore.Application.QueriesAndCommands.Logging._1Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Logging._2Queries
{
    public class WebLogGetLastestQueryHandler : BaseCoreClass, IRequestHandler<WebLogGetLastestQuery, WebLogGetLastestResponse>
    {
        private readonly WebLogManager _manager;

        public WebLogGetLastestQueryHandler(ICoreServices coreServices, WebLogManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<WebLogGetLastestResponse> Handle(WebLogGetLastestQuery request, CancellationToken cancellationToken)
        {
            var ll = _manager.GetLatestWebLogs();

            return await Task.FromResult(new WebLogGetLastestResponse { LatesWebLogLines = ll.LatesWebLogLines });
        }
    }
}
