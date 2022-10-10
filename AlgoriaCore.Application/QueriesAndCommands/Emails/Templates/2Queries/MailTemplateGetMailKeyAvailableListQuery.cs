using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._2Queries
{
    public  class MailTemplateGetMailKeyAvailableListQuery : IRequest<List<ComboboxItemDto>>
    {
        public long MailGroup { get; set; }
    }
}
