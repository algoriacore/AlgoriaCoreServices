using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.OrgUnits;
using AlgoriaCore.Application.Managers.OrgUnits.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnitUsers.OrgUnitUsers
{
    public class OrgUnitUserGetListQueryHandler : BaseCoreClass, IRequestHandler<OrgUnitUserGetListQuery, PagedResultDto<OrgUnitUserForListResponse>>
    {
        private readonly OrgUnitManager _manager;

        public OrgUnitUserGetListQueryHandler(ICoreServices coreServices, OrgUnitManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<OrgUnitUserForListResponse>> Handle(OrgUnitUserGetListQuery request, CancellationToken cancellationToken)
        {
            OrgUnitUserListFilterDto filterDto = new OrgUnitUserListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                OrgUnit = request.OrgUnit
            };

            PagedResultDto<OrgUnitUserDto> pagedResultDto = await _manager.GetOrgUnitUserListAsync(filterDto);
            List<OrgUnitUserForListResponse> ll = new List<OrgUnitUserForListResponse>();

            foreach (OrgUnitUserDto dto in pagedResultDto.Items)
            {
                ll.Add(new OrgUnitUserForListResponse()
                {
                    Id = dto.Id.Value,
                    OrgUnit = dto.OrgUnit,
                    OrgUnitDesc = dto.OrgUnitDesc,
                    User = dto.User,
                    UserDesc = dto.UserDesc
                });
            }
            return new PagedResultDto<OrgUnitUserForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
