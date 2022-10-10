using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnitUsers.OrgUnitUsers
{
    public class OrgUnitUserCreateCommand : IRequest<long>
    {
        public long OrgUnit { get; set; }
        public long User { get; set; }
    }
}