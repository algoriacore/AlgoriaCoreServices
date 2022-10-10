using AlgoriaCore.Application.QueriesAndCommands.Users._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._2Queries
{
    public class UserGetForEditQuery : IRequest<UserForEditResponse>
    {
        public long Id { get; set; }
    }
}
