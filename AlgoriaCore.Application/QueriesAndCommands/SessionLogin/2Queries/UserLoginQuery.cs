using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.SessionLogin._2Queries
{
    public class UserLoginQuery : IRequest<SessionLoginResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TenancyName { get; set; }
    }
}

