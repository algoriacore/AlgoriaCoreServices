using AlgoriaCore.Application.BaseClases.Dto;

namespace AlgoriaCore.Application.Managers.Templates.Dto
{
    public class TemplateFieldListFilterDto: PageListByDto
    {
        public bool OnlyProcessed { get; set; }
        public long? Template { get; set; }
        public long? TemplateSection { get; set; }
    }
}
