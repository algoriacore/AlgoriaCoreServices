using System;
using MediatR;
using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailAttachs;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailAttachs.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMailAttachs
{
    public class MailServiceMailAttachGetListQueryHandler : BaseCoreClass, IRequestHandler<MailServiceMailAttachGetListQuery, PagedResultDto<MailServiceMailAttachListResponse>>
    {
        private readonly MailServiceMailAttachManager _manager;
        public MailServiceMailAttachGetListQueryHandler(ICoreServices coreServices, MailServiceMailAttachManager manager) : base(coreServices)
        {
            _manager = manager;
        }
        public async Task<PagedResultDto<MailServiceMailAttachListResponse>> Handle(MailServiceMailAttachGetListQuery request, CancellationToken cancellationToken)
        {
            MailServiceMailAttachListFilterDto filterDto = new MailServiceMailAttachListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                MailServiceMailBody = request.MailServiceMailBody
            };

            PagedResultDto<MailServiceMailAttachDto> pagedResultDto = await _manager.GetMailServiceMailAttachPagedListAsync(filterDto);
            List<MailServiceMailAttachListResponse> ll = new List<MailServiceMailAttachListResponse>();

            foreach (MailServiceMailAttachDto dto in pagedResultDto.Items)
            {
                ll.Add(new MailServiceMailAttachListResponse()
                {
                    Id = dto.Id,
                    ContenType = dto.ContenType,
                    FileName = dto.FileName,
                });
            }

            return new PagedResultDto<MailServiceMailAttachListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}

