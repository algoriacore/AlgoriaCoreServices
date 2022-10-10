using System;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomResponse
    {
        public string Id { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public string CollectionName { get; set; }
        public string Description { get; set; }
        public bool IsCollectionGenerated { get; set; }
        public string NamePlural { get; set; }
        public string NameSingular { get; set; }
        public string UserCreator { get; set; }
        public bool IsActive { get; set; }

        public string Questionnaire { get; set; }

        public List<string> FieldNames { get; set; }

        public CatalogCustomResponse()
        {
            FieldNames = new List<string>();
        }
    }
}