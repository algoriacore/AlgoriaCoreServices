using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class ConfirmPasswordCommandReset : IRequest<long>
    {
        public string ConfirmationCode { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
