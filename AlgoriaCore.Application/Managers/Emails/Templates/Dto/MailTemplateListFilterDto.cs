using AlgoriaCore.Application.BaseClases.Dto;

namespace AlgoriaCore.Application.Managers.Emails.Templates.Dto
{
    public class MailTemplateListFilterDto : PageListByDto
    {
        public long MailGroup { get; set; }

        public bool IsIncludeBody { get; set; }
    }
}
