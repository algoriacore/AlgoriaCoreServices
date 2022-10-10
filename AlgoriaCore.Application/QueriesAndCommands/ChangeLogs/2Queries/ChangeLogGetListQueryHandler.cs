using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.ChangeLogs;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.ChangeLogs
{
    public class ChangeLogGetListQueryHandler : BaseCoreClass, IRequestHandler<ChangeLogGetListQuery, PagedResultDto<ChangeLogForListResponse>>
    {
        private readonly ChangeLogManager _managerChangeLog;

        public ChangeLogGetListQueryHandler(ICoreServices coreServices
        , ChangeLogManager managerChangeLog)
                                : base(coreServices)
        {
            _managerChangeLog = managerChangeLog;
        }

        public async Task<PagedResultDto<ChangeLogForListResponse>> Handle(ChangeLogGetListQuery request, CancellationToken cancellationToken)
        {
            ChangeLogListFilterDto filterDto = new ChangeLogListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                Table = request.Table,
                Key = request.Key
            };

            PagedResultDto<ChangeLogDto> pagedResultDto = await _managerChangeLog.GetChangeLogListAsync(filterDto);
            List<ChangeLogForListResponse> ll = new List<ChangeLogForListResponse>();

            foreach(ChangeLogDto dto in pagedResultDto.Items)
            {
                ll.Add(new ChangeLogForListResponse()
                {
                    Id = dto.Id,
                    User = dto.User,
                    UserDesc = dto.UserDesc,
                    Datetime = dto.Datetime,
                    Detail = dto.Detail
                });
            }

            return new PagedResultDto<ChangeLogForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
