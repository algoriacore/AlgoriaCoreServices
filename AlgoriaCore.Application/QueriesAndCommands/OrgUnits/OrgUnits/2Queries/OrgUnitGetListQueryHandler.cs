using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.OrgUnits;
using AlgoriaCore.Application.Managers.OrgUnits.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitGetListQueryHandler : BaseCoreClass, IRequestHandler<OrgUnitGetListQuery, PagedResultDto<OrgUnitForListResponse>>
    {
        private readonly OrgUnitManager _manager;

        public OrgUnitGetListQueryHandler(ICoreServices coreServices, OrgUnitManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<OrgUnitForListResponse>> Handle(OrgUnitGetListQuery request, CancellationToken cancellationToken)
        {
            OrgUnitListFilterDto filterDto = new OrgUnitListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                ParentOU = request.ParentOU,
                Level = request.Level
            };

            PagedResultDto<OrgUnitDto> pagedResultDto = await _manager.GetOrgUnitListAsync(filterDto);
            List<OrgUnitForListResponse> ll = new List<OrgUnitForListResponse>();

            foreach (OrgUnitDto dto in pagedResultDto.Items)
            {
                ll.Add(new OrgUnitForListResponse()
                {
                    Id = dto.Id.Value,
                    ParentOU = dto.ParentOU,
                    ParentOUDesc = dto.ParentOUDesc,
                    Name = dto.Name,
                    Level = dto.Level,
                    HasChildren = dto.HasChildren,
                    Size = dto.Size
                });
            }

            return new PagedResultDto<OrgUnitForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
