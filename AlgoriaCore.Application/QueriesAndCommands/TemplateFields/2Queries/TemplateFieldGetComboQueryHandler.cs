using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldGetComboQueryHandler : BaseCoreClass, IRequestHandler<TemplateFieldGetComboQuery, List<ComboboxItemDto>>
    {
        private readonly TemplateManager _manager;

        public TemplateFieldGetComboQueryHandler(ICoreServices coreServices, TemplateManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<ComboboxItemDto>> Handle(TemplateFieldGetComboQuery request, CancellationToken cancellationToken)
        {
            return await _manager.GetTemplateFieldComboAsync(new TemplateFieldComboFilterDto() { Template = request.Template });
        }
    }
}
