using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Templates;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._2Queries
{
    public class MailTemplateGetMailKeyAvailableListQueryHandler : BaseCoreClass, IRequestHandler<MailTemplateGetMailKeyAvailableListQuery, List<ComboboxItemDto>>
    {
        private readonly MailTemplateManager _manager;

        public MailTemplateGetMailKeyAvailableListQueryHandler(ICoreServices coreServices, MailTemplateManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<ComboboxItemDto>> Handle(MailTemplateGetMailKeyAvailableListQuery request, CancellationToken cancellationToken)
        {
			var ls = await _manager.GetMailTemplateMailKeyAvailableList(request.MailGroup);
			var ll = new List<ComboboxItemDto>();

			ls = ls.OrderBy(m => m.Label).ToList();

			foreach (var item in ls)
			{
				ll.Add(new ComboboxItemDto(item.Value, item.Label));
			}

			return ll;
		}
    }
}
