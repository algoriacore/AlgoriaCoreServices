using AlgoriaCore.Application.BaseClases.Dto;

namespace AlgoriaCore.Application.Managers.Roles.Dto
{
    public class RoleListFilterDto : PageListByDto
    {
        public string Name { get; set; }
    }
}
