using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageUpdateCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
    }
}