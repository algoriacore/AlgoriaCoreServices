using AlgoriaCore.Application.BaseClases.Dto;

namespace AlgoriaCore.Application.Managers.Templates.Dto
{
    public class TemplateSectionListFilterDto: PageListByDto
    {
        public long? Template { get; set; }
    }
}
