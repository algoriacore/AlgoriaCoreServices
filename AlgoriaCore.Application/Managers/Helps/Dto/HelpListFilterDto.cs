using AlgoriaCore.Application.BaseClases.Dto;

namespace AlgoriaCore.Application.Managers.Helps.Dto
{
    public class HelpListFilterDto: PageListByDto
    {
        public bool IsIncludeBody { get; set; }
    }
}
