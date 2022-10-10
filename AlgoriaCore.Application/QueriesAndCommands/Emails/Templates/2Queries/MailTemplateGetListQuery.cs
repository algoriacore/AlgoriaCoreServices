using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._2Queries
{
    public class MailTemplateGetListQuery : PageListByDto, IRequest<PagedResultDto<MailTemplateListResponse>>
    {
        public long MailGroup { get; set; }
    }
}
