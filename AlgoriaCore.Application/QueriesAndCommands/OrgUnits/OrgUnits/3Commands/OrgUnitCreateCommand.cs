using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitCreateCommand : IRequest<long>
    {
        public long? ParentOU { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }
    }
}