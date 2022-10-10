using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants
{
    public class GetTenantQuery : IRequest<TenantResponse>
    {
        public int Id { get; set; }
    }
}
