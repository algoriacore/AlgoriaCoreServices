using AlgoriaCore.Application.QueriesAndCommands.Roles._1Model;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries
{
    public class RoleGetForListActiveQuery : IRequest<List<RoleForListActiveResponse>>
    {
    }
}
