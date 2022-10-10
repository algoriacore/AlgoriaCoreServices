using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplGetByIdQuery : IRequest<CatalogCustomImplResponse>
    {
        public string Catalog { get; set; }
        public string Id { get; set; }
    }
}
