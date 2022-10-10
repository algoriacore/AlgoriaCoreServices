using AlgoriaCore.Application.Managers.Emails.Templates.Dto;
using System.Collections.Generic;

namespace AlgoriaCore.Application.Managers.Emails.Groups.Dto
{
    public class MailGroupDto
    {
        public long? Id { get; set; }
        public string DisplayName { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public int? TenantId { get; set; }

        public bool? IsSelected { get; set; }

        public List<MailTemplateDto> TemplateList { get; set; }
    }
}
