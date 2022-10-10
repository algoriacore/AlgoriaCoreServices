using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplGetForReadQuery : IRequest<CatalogCustomImplForReadResponse>
    {
        public string Catalog { get; set; }
        public string Id { get; set; }
    }
}
