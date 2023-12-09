using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._1Model
{
    public class DynamicCatalogRelationResponse
    {
        public long Id { get; set; }
        public string CampoRelacion { get; set; }
        public string TablaRelacionada { get; set; }
        public string CampoReferenciado { get; set; }
        public string CampoDescReferenciado { get; set; }
    }
}
