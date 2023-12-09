using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos.Dto
{
    public class DynamicCatalogRelationDto
    {
        public long Id { get; set; }
        public string CampoRelacion { get; set; }
        public string TablaRelacionada { get; set; }
        public string CampoReferenciado { get; set; }
        public string CampoDescReferenciado { get; set; }
        public bool TablaReferenciadaTieneIsActive { get; set; }
        public bool TablaReferenciadaTieneIsDeleted { get; set; }
    }
}
