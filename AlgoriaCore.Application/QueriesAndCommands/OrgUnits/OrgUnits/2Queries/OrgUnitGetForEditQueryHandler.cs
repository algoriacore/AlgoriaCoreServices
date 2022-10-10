using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.OrgUnits;
using AlgoriaCore.Application.Managers.OrgUnits.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitGetForEditQueryHandler : BaseCoreClass, IRequestHandler<OrgUnitGetForEditQuery, OrgUnitForEditResponse>
    {
        private readonly OrgUnitManager _manager;

        public OrgUnitGetForEditQueryHandler(ICoreServices coreServices, OrgUnitManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<OrgUnitForEditResponse> Handle(OrgUnitGetForEditQuery request, CancellationToken cancellationToken)
        {
            OrgUnitForEditResponse response;

            if (request.Id.HasValue)
            {
                OrgUnitDto dto = await _manager.GetOrgUnitAsync(request.Id.Value);

                response = new OrgUnitForEditResponse()
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
            else
            {
                response = new OrgUnitForEditResponse();
            }

            return response;
        }
    }
}
