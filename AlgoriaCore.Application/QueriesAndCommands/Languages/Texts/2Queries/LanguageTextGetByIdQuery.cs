using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Texts
{
    public class LanguageTextGetByIdQuery : IRequest<LanguageTextResponse>
    {
        public long Id { get; set; }
    }
}
