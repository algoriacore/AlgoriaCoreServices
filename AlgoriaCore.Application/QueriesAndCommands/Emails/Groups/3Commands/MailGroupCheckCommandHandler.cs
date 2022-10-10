using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Groups;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._3Commands
{
    public class MailGroupCheckCommandHandler : BaseCoreClass, IRequestHandler<MailGroupCheckCommand, long>
    {
        private readonly MailGroupManager _manager;

        public MailGroupCheckCommandHandler(ICoreServices coreServices, MailGroupManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(MailGroupCheckCommand request, CancellationToken cancellationToken)
        {
            await _manager.CheckMailGroup(request.Id);

            return request.Id;
        }
    }
}
