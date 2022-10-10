using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireGetListQuery : PageListByDto, IRequest<PagedResultDto<QuestionnaireForListResponse>>
    {

    }
}
