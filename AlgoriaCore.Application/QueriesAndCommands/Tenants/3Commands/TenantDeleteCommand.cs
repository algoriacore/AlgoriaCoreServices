using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants._3Commands
{
    public class TenantDeleteCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
