using AlgoriaCore.Domain.Attributes;
using MediatR;
using System.Dynamic;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    [MongoTransactional]
    public class CatalogCustomImplUpdateCommand : IRequest<string>
    {
        public string Catalog { get; set; }
        public string Id { get; set; }
        public ExpandoObject Data { get; set; }
    }
}