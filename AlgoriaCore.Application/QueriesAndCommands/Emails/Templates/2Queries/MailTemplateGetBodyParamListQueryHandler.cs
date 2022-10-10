using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Templates;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._2Queries
{
    public class MailTemplateGetBodyParamListQueryHandler : BaseCoreClass, IRequestHandler<MailTemplateGetBodyParamListQuery, List<ComboboxItemDto>>
    {
        private readonly MailTemplateManager _manager;

        public MailTemplateGetBodyParamListQueryHandler(ICoreServices coreServices, MailTemplateManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<ComboboxItemDto>> Handle(MailTemplateGetBodyParamListQuery request, CancellationToken cancellationToken)
        {
            var ls = _manager.GetMailTemplateBodyParamList(request.MailKey);
            var ll = new List<ComboboxItemDto>();

            foreach (var item in ls)
            {
                ll.Add(new ComboboxItemDto(item, item));
            }

            return await Task.FromResult(ll);
        }
    }
}
