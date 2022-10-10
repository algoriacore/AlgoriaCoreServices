using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitGetByIdQuery : IRequest<OrgUnitResponse>
    {
        public long Id { get; set; }
    }
}
