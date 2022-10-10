using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Templates;
using AlgoriaCore.Application.Managers.Emails.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._3Commands
{
    public class MailTemplateSendTestCommandHandler : BaseCoreClass, IRequestHandler<MailTemplateSendTestCommand, int>
    {
        private readonly MailTemplateManager _manager;

        public MailTemplateSendTestCommandHandler(ICoreServices coreServices, MailTemplateManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<int> Handle(MailTemplateSendTestCommand request, CancellationToken cancellationToken)
        {
            var dto = new MailTemplateSendTestDto
            {
                MailGroup = request.MailGroup,
                Email = request.Email,
                Subject = request.Subject,
                Body = request.Body
            };

            await _manager.SendTestMailAsync(dto);

            return 0;
        }
    }
}
