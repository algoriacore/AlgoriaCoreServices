using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._1Model
{
    public class DynamicCatalogValidationResponse
    {
        public long Id { get; set; }
        public string Campo { get; set; }
        public int Regla { get; set; }
        public string ValorReferencia { get; set; }
    }
}
