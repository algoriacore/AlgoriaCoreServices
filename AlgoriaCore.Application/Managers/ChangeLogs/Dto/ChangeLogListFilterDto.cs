using AlgoriaCore.Application.BaseClases.Dto;

namespace AlgoriaCore.Application.Managers.ChangeLogs.Dto
{
    public class ChangeLogListFilterDto : PageListByDto
    {
        public string Table { get; set; }
        public string Key { get; set; }
    }
}
