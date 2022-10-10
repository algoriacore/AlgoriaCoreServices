using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomGetForEditQuery : IRequest<CatalogCustomForEditResponse>
    {
        public string Id { get; set; }
    }
}
