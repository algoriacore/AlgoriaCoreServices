using AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._1Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos.Dto
{
    public class DynamicCatalogFormInfoDto
    {
        public string CampoLlavePrimaria { get; set; }
        public List<DynamicCatalogDefinitionDto> DefinitionsList { get; set; }
        public List<DynamicCatalogValidationDto> ValidationsList { get; set; }
        public List<DynamicCatalogRelationDto> RelationsList { get; set; }
    }
}
