using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos.Dto
{
    public class DynamicCatalogDto
    {
        public long Id { get; set; }
        public string Tabla { get; set; }
        public bool TieneTenantId { get; set; }
        public bool TieneIsDeleted { get; set; }
        public string Descripcion { get; set; }
        public bool MetadatosGenerados { get; set; }
        public bool IsActive { get; set; }
        public string CampoLlavePrimaria { get; set; }
    }
}
