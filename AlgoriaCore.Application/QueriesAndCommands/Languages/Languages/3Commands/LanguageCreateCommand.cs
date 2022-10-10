using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageCreateCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
    }
}