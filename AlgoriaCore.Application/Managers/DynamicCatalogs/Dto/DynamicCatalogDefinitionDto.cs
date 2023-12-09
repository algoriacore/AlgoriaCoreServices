using AlgoriaCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos.Dto
{
    public class DynamicCatalogDefinitionDto
    {
        public long Id { get; set; }
        public string Campo { get; set; }
        public string Tipo { get; set; }
        public int? Longitud { get; set; }
        public int? Decimales { get; set; }
        public bool EsLlavePrimaria { get; set; }
        public bool MostrarEnVista { get; set; }
        public bool CapturarEnPantalla { get; set; }
        public int Posicion { get; set; }
    }
}
