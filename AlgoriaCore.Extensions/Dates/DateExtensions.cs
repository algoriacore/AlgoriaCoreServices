using System;

namespace AlgoriaCore.Extensions.Dates
{
    public static class DateExtensions
    {
        public static DateTimeRange ToRange(this DateTime value)
        {
            DateTimeRange ret = new DateTimeRange();

            var f1 = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, DateTimeKind.Unspecified);
            var f2 = f1.AddDays(1).AddSeconds(-1);

            ret.StartDate = f1;
            ret.EndDate = f2;

            return ret;
        }
    }

    public class DateTimeRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
