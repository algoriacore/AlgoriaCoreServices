using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._1Model
{
    public class DynamicCatalogFormInfoResponse
    {
        public List<DynamicCatalogDefinitionResponse> DefinitionsList { get; set; }
        public List<DynamicCatalogValidationResponse> ValidationsList { get; set; }
        public List<DynamicCatalogRelationResponse> RelationsList { get; set; }
        public string CampoLlavePrimaria { get; set; }
    }
}
