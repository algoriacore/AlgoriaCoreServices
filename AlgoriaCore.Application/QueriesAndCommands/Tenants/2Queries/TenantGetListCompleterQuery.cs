using AlgoriaCore.Application.QueriesAndCommands.Tenants._1Model;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants._2Queries
{
    public class TenantGetListCompleterQuery : IRequest<List<TenantListResponse>>
    {
        public string Filter { get; set; }
    }
}
