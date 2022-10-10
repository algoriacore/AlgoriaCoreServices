using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitDeleteCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}