using System;

namespace AlgoriaCore.Domain.Interfaces.Date
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}
