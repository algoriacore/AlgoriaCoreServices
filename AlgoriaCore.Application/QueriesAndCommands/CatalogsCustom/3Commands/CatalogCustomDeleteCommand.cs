using AlgoriaCore.Domain.Attributes;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    [MongoTransactional]
    public class CatalogCustomDeleteCommand : IRequest<string>
    {
        public string Id { get; set; }
    }
}