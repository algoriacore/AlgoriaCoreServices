using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.SessionLogin._2Queries
{
    public class UserLoginTokenQuery : IRequest<SessionLoginResponse>
    {
        public int? TenantId { get; set; }
        public long UserId { get; set; }
    }
}
