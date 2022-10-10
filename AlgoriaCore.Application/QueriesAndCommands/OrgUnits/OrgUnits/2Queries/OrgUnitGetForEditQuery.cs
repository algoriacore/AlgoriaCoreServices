using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitGetForEditQuery : IRequest<OrgUnitForEditResponse>
    {
        public long? Id { get; set; }
    }
}
