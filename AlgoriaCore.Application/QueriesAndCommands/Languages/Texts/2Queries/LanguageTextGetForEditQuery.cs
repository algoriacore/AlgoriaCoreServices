using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Texts
{
    public class LanguageTextGetForEditQuery : IRequest<LanguageTextForEditResponse>
    {
        public int? Id { get; set; }
    }
}
