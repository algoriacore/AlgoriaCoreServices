using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.OrgUnits;
using AlgoriaCore.Application.Managers.OrgUnits.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitGetByIdQueryHandler : BaseCoreClass, IRequestHandler<OrgUnitGetByIdQuery, OrgUnitResponse>
    {
        private readonly OrgUnitManager _manager;

        public OrgUnitGetByIdQueryHandler(ICoreServices coreServices, OrgUnitManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<OrgUnitResponse> Handle(OrgUnitGetByIdQuery request, CancellationToken cancellationToken)
        {
            OrgUnitDto dto = await _manager.GetOrgUnitAsync(request.Id);

            return new OrgUnitResponse()
            {
                Id = dto.Id.Value,
                ParentOU = dto.ParentOU,
                ParentOUDesc = dto.ParentOUDesc,
                Name = dto.Name,
                Level = dto.Level,
                HasChildren = dto.HasChildren,
                Size = dto.Size
            };
        }
    }
}
