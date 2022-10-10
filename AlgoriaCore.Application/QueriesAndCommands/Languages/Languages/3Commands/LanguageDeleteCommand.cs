using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageDeleteCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}