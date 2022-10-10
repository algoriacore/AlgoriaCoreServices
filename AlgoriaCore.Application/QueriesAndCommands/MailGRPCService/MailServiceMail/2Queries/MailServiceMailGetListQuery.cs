using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System;

namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMails
{
     public class MailServiceMailGetListQuery : PageListByDto, IRequest<PagedResultDto<MailServiceMailListResponse>>
     {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? TenantId { get; set; }

        public bool? OnlyHost { get; set; }
    }
}

