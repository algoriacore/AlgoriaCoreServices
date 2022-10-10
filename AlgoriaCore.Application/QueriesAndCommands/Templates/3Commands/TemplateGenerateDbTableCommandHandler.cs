using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateGenerateDbTableCommandHandler : BaseCoreClass, IRequestHandler<TemplateGenerateDbTableCommand, long>
    {
        private readonly TemplateManager _manager;

        public TemplateGenerateDbTableCommandHandler(ICoreServices coreServices, TemplateManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(TemplateGenerateDbTableCommand request, CancellationToken cancellationToken)
        {
            await _manager.GenerateDbTableAsync(request.Id);

            return request.Id;
        }
    }
}
