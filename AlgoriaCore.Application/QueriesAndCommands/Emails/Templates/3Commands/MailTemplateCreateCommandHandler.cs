using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Templates;
using AlgoriaCore.Application.Managers.Emails.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._3Commands
{
    public class MailTemplateCreateCommandHandler : BaseCoreClass, IRequestHandler<MailTemplateCreateCommand, long>
    {
        private readonly MailTemplateManager _manager;

        public MailTemplateCreateCommandHandler(ICoreServices coreServices, MailTemplateManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(MailTemplateCreateCommand request, CancellationToken cancellationToken)
        {
            var dto = new MailTemplateDto
            {
                MailGroup = request.MailGroup,
                MailKey = request.MailKey,
                DisplayName = request.DisplayName,
                SendTo = request.SendTo,
                CopyTo = request.CopyTo,
                BlindCopyTo = request.BlindCopyTo,
                Subject = request.Subject,
                Body = request.Body,
                IsActive = request.IsActive
            };

            return await _manager.CreateMailTemplateAsync(dto);
        }
    }
}
