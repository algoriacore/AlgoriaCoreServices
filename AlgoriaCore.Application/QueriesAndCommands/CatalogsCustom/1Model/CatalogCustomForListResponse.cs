using System;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomForListResponse
    {
        public string Id { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public string CollectionName { get; set; }
        public string Description { get; set; }
        public bool IsCollectionGenerated { get; set; }
        public string IsCollectionGeneratedDesc { get; set; }
        public string NamePlural { get; set; }
        public string NameSingular { get; set; }
        public string UserCreator { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }

        public string Questionnaire { get; set; }
    }
}