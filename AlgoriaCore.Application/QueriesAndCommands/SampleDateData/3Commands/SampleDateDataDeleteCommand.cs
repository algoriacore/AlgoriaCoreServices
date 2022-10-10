using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataDeleteCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}