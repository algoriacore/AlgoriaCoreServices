using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System;

namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMails
{
    public class MailServiceMailExportCSVQuery : PageListByDto, IRequest<FileDto>
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? TenantId { get; set; }

        public bool? OnlyHost { get; set; }

        public string ViewColumnsConfigJSON { get; set; }
    }
}
