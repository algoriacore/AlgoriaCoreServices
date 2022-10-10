using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Texts
{
    public class LanguageTextUpdateCommand : IRequest<long>
    {
        public long? Id { get; set; }
        public int LanguageId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}