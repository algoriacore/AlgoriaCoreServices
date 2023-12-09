using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos.Dto
{
    public class DynamicCatalogViewInfoDto
    {
        public string CampoLlavePrimaria { get; set; }
        public List<string> CamposVistaList { get; set; }
        public List<DynamicCatalogDefinitionDto> DefinicionesList { get; set; }
        public List<DynamicCatalogRelationDto> RelationsList { get; set; }
    }
}
