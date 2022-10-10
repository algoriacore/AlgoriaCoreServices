using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Groups;
using AlgoriaCore.Application.Managers.Emails.Groups.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._3Commands
{
    public class MailGroupUpdateCommandHandler : BaseCoreClass, IRequestHandler<MailGroupUpdateCommand, long>
    {
        private readonly MailGroupManager _manager;

        public MailGroupUpdateCommandHandler(ICoreServices coreServices, MailGroupManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(MailGroupUpdateCommand request, CancellationToken cancellationToken)
        {
            var dto = new MailGroupDto {
                Id = request.Id,
                DisplayName = request.DisplayName,
                Header = request.Header,
                Footer = request.Footer
            };

            return await _manager.UpdateMailGroupAsync(dto);
        }
    }
}
