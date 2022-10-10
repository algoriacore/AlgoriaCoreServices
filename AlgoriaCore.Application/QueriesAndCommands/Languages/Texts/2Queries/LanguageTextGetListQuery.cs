using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Texts
{
    public class LanguageTextGetListQuery : PageListByDto, IRequest<PagedResultDto<LanguageTextForListResponse>>
    {
        public int? LanguageId { get; set; }
    }
}
