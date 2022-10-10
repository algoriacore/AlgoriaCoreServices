using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Groups;
using AlgoriaCore.Application.Managers.Emails.Groups.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._3Commands
{
    public class MailGroupCopyCommandHandler : BaseCoreClass, IRequestHandler<MailGroupCopyCommand, long>
    {
        private readonly MailGroupManager _manager;

        public MailGroupCopyCommandHandler(ICoreServices coreServices, MailGroupManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(MailGroupCopyCommand request, CancellationToken cancellationToken)
        {
            var dto = new MailGroupDto {
                Id = request.Id,
                DisplayName = request.DisplayName
            };

            return await _manager.CopyMailGroupAsync(dto);
        }
    }
}
