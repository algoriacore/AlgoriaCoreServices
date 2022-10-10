using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataGetByIdQuery : IRequest<SampleDateDataResponse>
    {
        public long Id { get; set; }
    }
}
