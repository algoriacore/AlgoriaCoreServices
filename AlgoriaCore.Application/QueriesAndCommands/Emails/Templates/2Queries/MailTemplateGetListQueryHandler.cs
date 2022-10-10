using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Templates;
using AlgoriaCore.Application.Managers.Emails.Templates.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._1Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._2Queries
{
    public class MailTemplateGetListQueryHandler : BaseCoreClass, IRequestHandler<MailTemplateGetListQuery, PagedResultDto<MailTemplateListResponse>>
    {
        private readonly MailTemplateManager _manager;

        public MailTemplateGetListQueryHandler(ICoreServices coreServices, MailTemplateManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<MailTemplateListResponse>> Handle(MailTemplateGetListQuery request, CancellationToken cancellationToken)
        {
            var filter = new MailTemplateListFilterDto
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                MailGroup = request.MailGroup
            };

            var pagedResultDto = await _manager.GetMailTemplateListAsync(filter);
            var ll = new List<MailTemplateListResponse>();

            foreach (var item in pagedResultDto.Items)
            {
                ll.Add(new MailTemplateListResponse
                {
                    Id = item.Id.Value,
                    MailKeyDesc = item.MailKeyDesc,
                    DisplayName = item.DisplayName
                });
            }

            return new PagedResultDto<MailTemplateListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
