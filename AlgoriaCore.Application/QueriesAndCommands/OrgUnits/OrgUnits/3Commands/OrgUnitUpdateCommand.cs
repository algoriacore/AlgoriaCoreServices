using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitUpdateCommand : IRequest<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}