using AlgoriaCore.Application.BaseClases.Dto;

namespace AlgoriaCore.Application.Managers.Users.Dto
{
    public class UserListFilterDto: PageListByDto
    {
        public string UserName { get; set; }
    }
}
