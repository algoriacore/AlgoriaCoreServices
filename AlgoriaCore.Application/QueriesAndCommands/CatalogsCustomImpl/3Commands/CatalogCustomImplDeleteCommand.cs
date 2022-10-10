using AlgoriaCore.Domain.Attributes;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    [MongoTransactional]
    public class CatalogCustomImplDeleteCommand : IRequest<string>
    {
        public string Catalog { get; set; }
        public string Id { get; set; }
    }
}