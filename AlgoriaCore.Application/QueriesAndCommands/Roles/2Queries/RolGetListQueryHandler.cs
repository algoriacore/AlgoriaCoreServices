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
    public class RolGetListQueryHandler : BaseCoreClass, IRequestHandler<RolGetListQuery, PagedResultDto<RolForListResponse>>
    {
        private readonly RolManager _rolManager;

        public RolGetListQueryHandler(ICoreServices coreServices, RolManager rolManager) : base(coreServices)
        {
            _rolManager = rolManager;
        }

        public async Task<PagedResultDto<RolForListResponse>> Handle(RolGetListQuery request, CancellationToken cancellationToken)
        {
            RolListFilterDto filterDto = new RolListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting
            };

            PagedResultDto<RolDto> pagedResultDto = await _rolManager.GetRolesListAsync(filterDto);
            List<RolForListResponse> ll = new List<RolForListResponse>();

            foreach (RolDto dto in pagedResultDto.Items)
            {
                ll.Add(new RolForListResponse()
                {
                    Id = dto.Id.Value,
                    Name = dto.Name,
                    DisplayName = dto.DisplayName,
                    IsActive = dto.IsActive,
                    IsActiveDesc = dto.IsActiveDesc
                });
            }

            return new PagedResultDto<RolForListResponse>(pagedResultDto.TotalCount, ll);
        }




    }
}
