using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._2Queries
{
    public class MailGroupGetListQuery : PageListByDto, IRequest<PagedResultDto<MailGroupListResponse>>
    {
    }
}
