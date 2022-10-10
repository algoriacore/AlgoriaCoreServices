using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomGetByIdQuery : IRequest<CatalogCustomResponse>
    {
        public string Id { get; set; }
    }
}
