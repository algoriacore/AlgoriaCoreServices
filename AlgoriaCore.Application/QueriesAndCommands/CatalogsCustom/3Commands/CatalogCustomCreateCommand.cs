using AlgoriaCore.Domain.Attributes;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    [MongoTransactional]
    public class CatalogCustomCreateCommand : IRequest<string>
    {
        public string Description { get; set; }
        public string NamePlural { get; set; }
        public string NameSingular { get; set; }
        public string UserCreator { get; set; }
        public bool IsActive { get; set; }
        public string Questionnaire { get; set; }

        public List<string> FieldNames { get; set; }

        public CatalogCustomCreateCommand()
        {
            FieldNames = new List<string>();
        }
    }
}