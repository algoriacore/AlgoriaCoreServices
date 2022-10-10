using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants
{
    public class UpdateTenantCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string TenancyName { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
