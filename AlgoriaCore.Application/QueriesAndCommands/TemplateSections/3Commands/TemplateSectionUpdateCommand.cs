using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionUpdateCommand : IRequest<long>
    {
        public long Id { get; set; }
        public long? Template { get; set; }
        public string Name { get; set; }
        public short? Order { get; set; }
        public string IconAF { get; set; }
    }
}