using System;

namespace AlgoriaCore.Domain.SqlFunctions
{
    public static class SqlFunctions
    {
        public static DateTime? AtTimeZone(DateTime? dt, string tz)
        => throw new InvalidOperationException($"{nameof(AtTimeZone)}cannot be called client side");

        public static DateTime? CreateDateTime(DateTime? dt, TimeSpan? ts)
        => throw new InvalidOperationException($"{nameof(CreateDateTime)}cannot be called client side");

        public static int? DateDiffCustom(string unit, DateTime? dt1, DateTime? dt2)
        => throw new InvalidOperationException($"{nameof(DateDiffCustom)}cannot be called client side");
    }
}
