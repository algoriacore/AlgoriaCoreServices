using System;
using MediatR;
using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMails;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMails.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMails
{
    public class MailServiceMailGetListQueryHandler : BaseCoreClass, IRequestHandler<MailServiceMailGetListQuery, PagedResultDto<MailServiceMailListResponse>>
    {
        private readonly MailServiceMailManager _manager;
        public MailServiceMailGetListQueryHandler(ICoreServices coreServices, MailServiceMailManager manager) : base(coreServices)
        {
            _manager = manager;
        }
        public async Task<PagedResultDto<MailServiceMailListResponse>> Handle(MailServiceMailGetListQuery request, CancellationToken cancellationToken)
        {
            MailServiceMailListFilterDto filterDto = new MailServiceMailListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,

                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TenantId = request.TenantId,
                OnlyHost = request.OnlyHost
            };


            PagedResultDto<MailServiceMailDto> pagedResultDto = null;
            List<MailServiceMailListResponse> ll = new List<MailServiceMailListResponse>();

            if (SessionContext.TenantId.HasValue)
            {
                pagedResultDto = await _manager.GetMailServiceMailPagedListAsync(filterDto);
            }
            else
            {
                pagedResultDto = await _manager.GetMailServiceMailPagedListByHostAsync(filterDto);
            }

            foreach (MailServiceMailDto dto in pagedResultDto.Items)
            {
                ll.Add(new MailServiceMailListResponse()
                {
                    Id = dto.Id,
                    MailServiceRequest = dto.MailServiceRequest,
                    MailServiceRequestDate = dto.MailServiceRequestDate,
                    IsLocalConfig = dto.IsLocalConfig,
                    IsLocalConfigDesc = dto.IsLocalConfigDesc,
                    Sendto = dto.Sendto,
                    CopyTo = dto.CopyTo,
                    Subject = dto.Subject,
                    Status = dto.Status,
                    StatusDesc = dto.StatusDesc
                });
            }

            return new PagedResultDto<MailServiceMailListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}

