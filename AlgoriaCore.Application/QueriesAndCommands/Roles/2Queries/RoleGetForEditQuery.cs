using AlgoriaCore.Application.QueriesAndCommands.Roles._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries
{
    public class RoleGetForEditQuery : IRequest<RoleForEditReponse>
    {
        public long Id { get; set; }
    }
}
