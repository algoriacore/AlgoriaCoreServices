using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._1Model
{
    public class RolForEditReponse
    {
        public long Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool? IsActive { get; set; }

        public List<RolPermisoResponse> PermisoList { get; set; }
    }
}
