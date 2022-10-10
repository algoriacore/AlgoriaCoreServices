﻿using AlgoriaCore.Application.BaseClases.Dto;
using System;

namespace AlgoriaCore.Application.Managers.Auditing.Dto
{
    public class AuditLogListFilterDto : PageListByDto
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string UserName { get; set; }

        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string BrowserInfo { get; set; }

        public int? TenantId { get; set; }

        public bool? HasException { get; set; }

        public int? MinExecutionDuration { get; set; }

        public int? MaxExecutionDuration { get; set; }

        public short? Severity { get; set; }

        public bool? OnlyHost { get; set; }
    }
}
