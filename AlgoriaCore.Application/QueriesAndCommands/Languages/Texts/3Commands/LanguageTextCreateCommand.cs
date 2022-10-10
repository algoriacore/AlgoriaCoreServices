using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Texts
{
    public class LanguageTextCreateCommand : IRequest<long>
    {
        public int LanguageId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}