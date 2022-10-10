using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateGetNoPagedListQuery : IRequest<List<TemplateForListResponse>>
    {

    }
}
