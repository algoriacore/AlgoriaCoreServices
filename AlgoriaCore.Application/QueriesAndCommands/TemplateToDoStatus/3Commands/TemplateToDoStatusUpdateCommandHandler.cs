using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusUpdateCommandHandler : BaseCoreClass, IRequestHandler<TemplateToDoStatusUpdateCommand, long>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateToDoStatusUpdateCommandHandler(ICoreServices coreServices
        , TemplateManager managerTemplate): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<long> Handle(TemplateToDoStatusUpdateCommand request, CancellationToken cancellationToken)
        {
            TemplateToDoStatusDto dto = new TemplateToDoStatusDto()
            {
                Id = request.Id,
                Type = request.Type,
                Name = request.Name,
                IsDefault = request.IsDefault,
                IsActive = request.IsActive
            };

            await _managerTemplate.UpdateTemplateToDoStatusAsync(dto);

            return dto.Id.Value;
        }
    }
}
