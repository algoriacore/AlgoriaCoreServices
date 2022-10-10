using System;

namespace AlgoriaCore.Application.Date
{
    public class Clock : Domain.Interfaces.Date.IClock
    {
        public DateTime Now { get { return DateTime.UtcNow; } }
    }
}
