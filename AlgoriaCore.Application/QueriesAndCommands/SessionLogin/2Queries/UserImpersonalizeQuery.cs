using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.SessionLogin._2Queries
{
    public class UserImpersonalizeQuery : IRequest<SessionLoginResponse>
    {
        public int? Tenant { get; set; }
        public long User { get; set; }
    }
}

