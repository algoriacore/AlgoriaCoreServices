using AlgoriaCore.Domain.Attributes;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Authorization.Permissions
{
    [Auditable(false)]
    public class PermissionGetIsGrantedQuery : IRequest<bool>
    {
        public bool RequiresAll { get; set; }
        public string[] PermissionNames { get; set; }
        public bool IsTemplateProcess { get; set; }
        public int? Template { get; set; }
        public bool IsCatalogCustomImpl { get; set; }
        public string Catalog { get; set; }
    }
}
