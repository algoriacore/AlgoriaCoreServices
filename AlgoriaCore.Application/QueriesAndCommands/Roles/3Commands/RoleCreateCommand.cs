using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._3Commands
{
    public class RoleCreateCommand : IRequest<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}
