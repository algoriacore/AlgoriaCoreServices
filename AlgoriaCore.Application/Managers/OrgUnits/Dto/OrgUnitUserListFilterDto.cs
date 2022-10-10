using AlgoriaCore.Application.BaseClases.Dto;

namespace AlgoriaCore.Application.Managers.OrgUnits.Dto
{
    public class OrgUnitUserListFilterDto: PageListByDto
    {
        public long? OrgUnit { get; set; }
    }
}
