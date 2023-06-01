using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.SessionLogin._2Queries
{
    public class UserLoginMicrosoftQuery : IRequest<SessionLoginResponse>
    {
        public string Token { get; set; }

        public string UserName { get; set; }
        public string TenancyName { get; set; }
    }
}

