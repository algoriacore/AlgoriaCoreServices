using AlgoriaCore.Domain.Attributes;
using MediatR;
using System.Dynamic;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    [MongoTransactional]
    public class CatalogCustomImplCreateCommand : IRequest<string>
    {
        public string Catalog { get; set; }
        public ExpandoObject Data { get; set; }
    }
}