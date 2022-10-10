using AlgoriaCore.Application.BaseClases.Dto;

namespace AlgoriaCore.Application.Managers.OrgUnits.Dto
{
    public class OrgUnitListFilterDto: PageListByDto
    {
        public long? ParentOU { get; set; }
        public byte? Level { get; set; }
    }
}
