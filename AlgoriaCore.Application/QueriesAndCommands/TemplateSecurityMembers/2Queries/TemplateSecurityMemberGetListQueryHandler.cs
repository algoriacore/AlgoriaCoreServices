using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSecurityMembers
{
    public class TemplateSecurityMemberGetListQueryHandler : BaseCoreClass, IRequestHandler<TemplateSecurityMemberGetListQuery, PagedResultDto<TemplateSecurityMemberForListResponse>>
    {
        private readonly TemplateManager _manager;

        public TemplateSecurityMemberGetListQueryHandler(ICoreServices coreServices, TemplateManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<TemplateSecurityMemberForListResponse>> Handle(TemplateSecurityMemberGetListQuery request, CancellationToken cancellationToken)
        {
            TemplateSecurityMemberListFilterDto filterDto = new TemplateSecurityMemberListFilterDto()
            {
                Filter = request.Filter,
                IsPaged = request.IsPaged,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                Template = request.Template,
                Type = request.Type,
                Level = request.Level
            };

            PagedResultDto<TemplateSecurityMemberDto> pagedResultDto = await _manager.GetTemplateSecurityMemberListAsync(filterDto);
            List<TemplateSecurityMemberForListResponse> ll = new List<TemplateSecurityMemberForListResponse>();

            foreach (TemplateSecurityMemberDto dto in pagedResultDto.Items)
            {
                ll.Add(new TemplateSecurityMemberForListResponse()
                {
                    Id = dto.Id.Value,
                    Template = dto.Template,
                    Type = dto.Type,
                    TypeDesc = dto.TypeDesc,
                    Member = dto.Member,
                    MemberDesc = dto.MemberDesc,
                    Level = dto.Level,
                    LevelDesc = dto.LevelDesc,
                    IsExecutor = dto.IsExecutor,
                    IsExecutorDesc = dto.IsExecutorDesc
                });
            }
            return new PagedResultDto<TemplateSecurityMemberForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
