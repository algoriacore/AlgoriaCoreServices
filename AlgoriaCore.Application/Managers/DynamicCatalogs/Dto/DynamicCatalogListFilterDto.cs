
using AlgoriaCore.Application.BaseClases.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos.Dto
{
    public class DynamicCatalogListFilterDto
    {
        public string Tabla { get; set; }
        public string Sorting { get; set; }
        public string Filter { get; set; }

        public int? PageSize { get; set; }

        public int? PageNumber { get; set; }
    }
}
