using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageGetForEditQuery : IRequest<LanguageForEditResponse>
    {
        public int? Id { get; set; }
    }
}
