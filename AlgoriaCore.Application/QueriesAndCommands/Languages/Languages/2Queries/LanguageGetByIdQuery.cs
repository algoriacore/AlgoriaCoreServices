using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageGetByIdQuery : IRequest<LanguageResponse>
    {
        public int Id { get; set; }
    }
}
