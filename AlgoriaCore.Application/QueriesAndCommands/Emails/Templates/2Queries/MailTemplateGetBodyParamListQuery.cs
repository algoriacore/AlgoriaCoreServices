using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._2Queries
{
    public class MailTemplateGetBodyParamListQuery : IRequest<List<ComboboxItemDto>>
    {
        public string MailKey { get; set; }
    }
}
