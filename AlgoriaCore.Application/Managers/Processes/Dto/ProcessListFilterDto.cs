using AlgoriaCore.Application.BaseClases.Dto;

namespace AlgoriaCore.Application.Managers.Processes.Dto
{
    public class ProcessListFilterDto : PageListByDto
    {
        public ProcessViewType ViewType { get; set; }

        public ProcessListFilterDto() {
            ViewType = ProcessViewType.Normal;
        }
    }

    public enum ProcessViewType: byte
    {
        Normal = 0,
        Own = 1,
        OwnPendings = 2
    }
}
