using AlgoriaCore.Domain.Attributes;
using AlgoriaCore.Domain.Authorization;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Authorization.Permissions
{
    //[MongoTransactional]
    public class PermissionGetTreeQuery : IRequest<Permission>
    {
    }
}
