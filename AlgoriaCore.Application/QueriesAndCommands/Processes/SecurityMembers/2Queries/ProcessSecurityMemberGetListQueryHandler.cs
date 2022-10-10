using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using AlgoriaCore.Application.Managers.Processes.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SecurityMembers
{
    public class ProcessSecurityMemberGetListQueryHandler : BaseCoreClass, IRequestHandler<ProcessSecurityMemberGetListQuery, PagedResultDto<ProcessSecurityMemberForListResponse>>
    {
        private readonly ProcessManager _manager;

        public ProcessSecurityMemberGetListQueryHandler(ICoreServices coreServices, ProcessManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<ProcessSecurityMemberForListResponse>> Handle(ProcessSecurityMemberGetListQuery request, CancellationToken cancellationToken)
        {
            await _manager.SetTemplate(request.Template);

            ProcessSecurityMemberListFilterDto filterDto = new ProcessSecurityMemberListFilterDto()
            {
                Filter = request.Filter,
                IsPaged = request.IsPaged,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                Parent = request.Parent,
                Type = request.Type,
                Level = request.Level
            };

            PagedResultDto<ProcessSecurityMemberDto> pagedResultDto = await _manager.GetProcessSecurityMemberListAsync(filterDto);
            List<ProcessSecurityMemberForListResponse> ll = new List<ProcessSecurityMemberForListResponse>();

            foreach (ProcessSecurityMemberDto dto in pagedResultDto.Items)
            {
                ll.Add(new ProcessSecurityMemberForListResponse()
                {
                    Id = dto.Id.Value,
                    Parent = dto.Parent,
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

            return new PagedResultDto<ProcessSecurityMemberForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
