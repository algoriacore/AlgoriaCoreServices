using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessGetComboQueryHandler : BaseCoreClass, IRequestHandler<ProcessGetComboQuery, List<ComboboxItemDto>>
    {
        private readonly ProcessManager _manager;

        public ProcessGetComboQueryHandler(ICoreServices coreServices, ProcessManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<ComboboxItemDto>> Handle(ProcessGetComboQuery request, CancellationToken cancellationToken)
        {
            return await _manager.GetProcessComboAsync(new ProcessComboFilterDto() { TemplateField = request.TemplateField, Filter = request.Filter });
        }
    }
}
