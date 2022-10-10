using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageSetDefaultCommand : IRequest<int>
    {
        public int Language { get; set; }
    }
}