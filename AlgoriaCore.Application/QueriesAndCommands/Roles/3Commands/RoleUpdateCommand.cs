using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._3Commands
{
    public class RoleUpdateCommand : IRequest<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}
