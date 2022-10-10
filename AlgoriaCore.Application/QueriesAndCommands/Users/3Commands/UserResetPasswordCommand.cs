using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserResetPasswordCommand: IRequest<string>
    {
        public string UserName { get; set; }
        public string TenancyName { get; set; }
    }
}
