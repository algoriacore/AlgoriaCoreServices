using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageGetListQuery : PageListByDto, IRequest<PagedResultDto<LanguageForListResponse>>
    {

    }
}
