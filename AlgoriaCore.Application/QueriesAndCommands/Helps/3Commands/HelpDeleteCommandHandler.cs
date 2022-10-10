using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Helps;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpDeleteCommandHandler : BaseCoreClass, IRequestHandler<HelpDeleteCommand, int>
    {
        private readonly HelpManager _manager;

        public HelpDeleteCommandHandler(ICoreServices coreServices, HelpManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<int> Handle(HelpDeleteCommand request, CancellationToken cancellationToken)
        {
            await _manager.DeleteHelpAsync(request.Id);

            return request.Id;
        }
    }
}
