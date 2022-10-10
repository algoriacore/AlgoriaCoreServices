using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataGetForEditQuery : IRequest<SampleDateDataForEditResponse>
    {
        public long? Id { get; set; }
    }
}
