using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomGetListQuery : PageListByDto, IRequest<PagedResultDto<CatalogCustomForListResponse>>
    {

    }
}
