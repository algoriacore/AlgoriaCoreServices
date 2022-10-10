using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Groups;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._3Commands
{
    public class MailGroupUnCheckCommandHandler : BaseCoreClass, IRequestHandler<MailGroupUnCheckCommand, long>
    {
        private readonly MailGroupManager _manager;

        public MailGroupUnCheckCommandHandler(ICoreServices coreServices, MailGroupManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(MailGroupUnCheckCommand request, CancellationToken cancellationToken)
        {
            await _manager.UnCheckMailGroup(request.Id);

            return request.Id;
        }
    }
}
