using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Roles;
using AlgoriaCore.Application.Managers.Roles.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Roles._1Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries
{
    public class RoleGetListQueryHandler : BaseCoreClass, IRequestHandler<RoleGetListQuery, PagedResultDto<RoleForListResponse>>
    {
        private readonly RoleManager _roleManager;

        public RoleGetListQueryHandler(ICoreServices coreServices, RoleManager rolManager) : base(coreServices)
        {
            _roleManager = rolManager;
        }

        public async Task<PagedResultDto<RoleForListResponse>> Handle(RoleGetListQuery request, CancellationToken cancellationToken)
        {
            RoleListFilterDto filterDto = new RoleListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting
            };

            PagedResultDto<RoleDto> pagedResultDto = await _roleManager.GetRolesListAsync(filterDto);
            List<RoleForListResponse> ll = new List<RoleForListResponse>();

            foreach (RoleDto dto in pagedResultDto.Items)
            {
                ll.Add(new RoleForListResponse()
                {
                    Id = dto.Id.Value,
                    Name = dto.Name,
                    DisplayName = dto.DisplayName,
                    IsActive = dto.IsActive,
                    IsActiveDesc = dto.IsActiveDesc
                });
            }

            return new PagedResultDto<RoleForListResponse>(pagedResultDto.TotalCount, ll);
        }




    }
}
