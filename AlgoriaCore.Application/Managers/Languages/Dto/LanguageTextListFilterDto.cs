using AlgoriaCore.Application.BaseClases.Dto;

namespace AlgoriaCore.Application.Managers.Languages.Dto
{
    public class LanguageTextListFilterDto: PageListByDto
    {
        public int? LanguageId { get; set; }
    }
}
