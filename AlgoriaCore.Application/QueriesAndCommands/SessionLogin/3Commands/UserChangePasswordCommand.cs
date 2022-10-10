using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.SessionLogin._3Commands
{
    public class UserChangePasswordCommand : IRequest<long>
    {
        public string TenancyName { get; set; }
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
