using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Templates;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._1Model;
using AlgoriaCore.Extensions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._2Queries
{
    public class MailTemplateGetForEditQueryHandler : BaseCoreClass, IRequestHandler<MailTemplateGetForEditQuery, MailTemplateForEditResponse>
    {
        private readonly MailTemplateManager _manager;

        public MailTemplateGetForEditQueryHandler(ICoreServices coreServices, MailTemplateManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<MailTemplateForEditResponse> Handle(MailTemplateGetForEditQuery request, CancellationToken cancellationToken)
        {
            var dto = await _manager.GetMailTemplate(request.Id);

            var resp = new MailTemplateForEditResponse
            {
                Id = dto.Id.Value,
                MailGroup = dto.MailGroup,
                MailKey = dto.MailKey,
                DisplayName = dto.DisplayName,
                SendTo = dto.SendTo,
                CopyTo = dto.CopyTo,
                BlindCopyTo = dto.BlindCopyTo,
                Subject = dto.Subject,
                Body = dto.Body,
                IsActive = dto.IsActive
            };

			var mailKeyList = await _manager.GetMailTemplateMailKeyAvailableList(dto.MailGroup ?? 0);
			resp.MailKeyList = mailKeyList.Select(s => new ComboboxItemDto(s.Value, s.Label)).ToList();

			if (!resp.MailKey.IsNullOrEmpty() && !resp.MailKeyList.Exists(p => p.Value == resp.MailKey))
			{
				var current = _manager.GetMailTemplateMailKeyList().FirstOrDefault(m => m.Value == resp.MailKey);
				if (current != null)
				{
					resp.MailKeyList.Add(new ComboboxItemDto(current.Value, current.Label));
					resp.MailKeyList = resp.MailKeyList.OrderBy(p => p.Label).ToList();
				}
			}

			return resp;
		}
    }
}
