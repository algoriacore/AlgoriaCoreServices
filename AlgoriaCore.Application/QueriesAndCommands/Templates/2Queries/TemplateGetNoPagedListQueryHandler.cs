using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateGetNoPagedListQueryHandler : BaseCoreClass, IRequestHandler<TemplateGetNoPagedListQuery, List<TemplateForListResponse>>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateGetNoPagedListQueryHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        ): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<List<TemplateForListResponse>> Handle(TemplateGetNoPagedListQuery request, CancellationToken cancellationToken)
        {
            List<TemplateDto> listDtos = await _managerTemplate.GetTemplateNoPagedListAsync();
            List<TemplateForListResponse> ll = new List<TemplateForListResponse>();

            foreach (TemplateDto dto in listDtos)
            {
                ll.Add(new TemplateForListResponse()
                {
                    Id = dto.Id,
                    RGBColor = dto.RGBColor,
                    NameSingular = dto.NameSingular,
                    NamePlural = dto.NamePlural,
                    Description = dto.Description,
                    Icon = dto.Icon,
                    IsTableGenerated = dto.IsTableGenerated,
                    TableName = dto.TableName,
                    HasChatRoom = dto.HasChatRoom,
                    IsActivity = dto.IsActivity,
                    HasSecurity = dto.HasSecurity,
                    IsActive = dto.IsActive
                });
            }

            return ll;
        }
    }
}
