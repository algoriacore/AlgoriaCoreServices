using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldGetListByTemplateQuery : IRequest<List<TemplateFieldResponse>>
    {
        public bool OnlyProcessed { get; set; }
        public long Template { get; set; }
    }
}
