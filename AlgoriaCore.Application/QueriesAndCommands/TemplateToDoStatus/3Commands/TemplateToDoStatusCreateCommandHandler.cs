using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusCreateCommandHandler : BaseCoreClass, IRequestHandler<TemplateToDoStatusCreateCommand, long>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateToDoStatusCreateCommandHandler(ICoreServices coreServices, TemplateManager managerTemplate): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<long> Handle(TemplateToDoStatusCreateCommand request, CancellationToken cancellationToken)
        {
            TemplateToDoStatusDto dto = new TemplateToDoStatusDto()
            {
                Template = request.Template,
                Type = request.Type,
                Name = request.Name,
                IsDefault = request.IsDefault,
                IsActive = request.IsActive
            };

            return await _managerTemplate.CreateTemplateToDoStatusAsync(dto);
        }
    }
}
