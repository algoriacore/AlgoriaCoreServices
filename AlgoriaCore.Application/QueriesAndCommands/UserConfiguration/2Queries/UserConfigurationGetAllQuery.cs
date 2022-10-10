using AlgoriaCore.Domain.Attributes;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.UserConfiguration
{
    [MongoTransactional]
    public class UserConfigurationGetAllQuery : IRequest<UserConfigurationResponse>
    {
        public string ClientType { get; set; }
    }
}
