using AlgoriaCore.Application.QueriesAndCommands.Users._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._2Queries
{
    public class UserProfileQuery : IRequest<UserForEditResponse>
    {
        public long Id { get; set; }
        public string ClientType { get; set; }
    }
}
