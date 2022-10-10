using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionCreateCommand : IRequest<long>
    {
        public long? Template { get; set; }
        public string Name { get; set; }
        public short? Order { get; set; }
        public string IconAF { get; set; }
    }
}