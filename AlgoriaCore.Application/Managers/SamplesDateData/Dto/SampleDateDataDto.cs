using System;

namespace AlgoriaCore.Application.Managers.SamplesDateData.Dto
{
    public class SampleDateDataDto
    {
        public long? Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public DateTime? DateTimeData { get; set; }
        public DateTime? DateData { get; set; }
        public TimeSpan? TimeData { get; set; }
    }
}
