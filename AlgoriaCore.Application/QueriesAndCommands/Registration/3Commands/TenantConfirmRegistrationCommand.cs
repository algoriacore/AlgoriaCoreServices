using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Registration._3Commands
{
    public class TenantConfirmRegistrationCommand : IRequest<int>
    {
        public string Code { get; set; }
    }
}
