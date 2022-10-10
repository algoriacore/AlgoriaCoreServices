using AlgoriaCore.Application.BaseClases.Dto;
using System;

namespace AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMails.Dto
{
     public class MailServiceMailListFilterDto: PageListByDto
     {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? TenantId { get; set; }

        public bool? OnlyHost { get; set; }
    }
}

