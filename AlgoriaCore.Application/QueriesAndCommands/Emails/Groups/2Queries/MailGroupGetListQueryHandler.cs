using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Groups;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._1Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._2Queries
{
    public class MailGroupGetListQueryHandler : BaseCoreClass, IRequestHandler<MailGroupGetListQuery, PagedResultDto<MailGroupListResponse>>
    {
        private readonly MailGroupManager _manager;

        public MailGroupGetListQueryHandler(ICoreServices coreServices, MailGroupManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<MailGroupListResponse>> Handle(MailGroupGetListQuery request, CancellationToken cancellationToken)
        {
            var filter = new PageListByDto
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting
            };

            var pagedResultDto = await _manager.GetMailGroupListAsync(filter);
            var ll = new List<MailGroupListResponse>();

            foreach (var item in pagedResultDto.Items)
            {
                ll.Add(new MailGroupListResponse
                {
                    Id = item.Id.Value,
                    DisplayName = item.DisplayName,
                    IsSelected = item.IsSelected
                });
            }

            return new PagedResultDto<MailGroupListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
