using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplGetForEditQuery : IRequest<CatalogCustomImplForEditResponse>
    {
        public string Catalog { get; set; }
        public string Id { get; set; }
    }
}
