using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnitUsers.OrgUnitUsers
{
    public class OrgUnitUserDeleteCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}