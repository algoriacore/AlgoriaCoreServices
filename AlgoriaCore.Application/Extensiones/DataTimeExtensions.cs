using NodaTime;
using NodaTime.Extensions;
using System;
using System.Linq;

namespace AlgoriaCore.Application.Extensiones
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convierte un DateTime no local a un DateTime local basado en la
        /// zona horaria especificada. El objeto retornado será Unspecified DateTimeKind 
        /// el cual representa un tiempo local agnóstico para servidores de zonas horarias. Para ser usado cuando
        /// queremos convertir UTC a tiempo local en cualquier lugar del mundo
        /// </summary>
        /// <param name="dateTime"> DateTime no local como UTC or Unspecified DateTimeKind.</param>
        /// <param name="timezone">Nombre de la zona horaria (en formato TZDB).</param>
        /// <returns>DateTime local como Unspecified DateTimeKind.</returns>
        public static DateTime ToZone(this DateTime dateTime, string timezone)
        {
            if (dateTime.Kind == DateTimeKind.Local)
            {
                throw new ArgumentException("Expected non-local kind of DateTime");
            }
            else if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }

            var zone = GetTimeZone(timezone);
            Instant instant = dateTime.ToInstant();
            ZonedDateTime inZone = instant.InZone(zone);
            DateTime unspecified = inZone.ToDateTimeUnspecified();

            return unspecified;
        }

        /// <summary>
        /// Convierte un DateTime local a un DateTime en UTC basado en la zona horaria
        /// especificada. El objeto retornado será UTC DateTimeKind. Para ser usado
        /// cuando queramos conocer cuál es la representación UTC del tiempo en cualquier lugar
        /// en el mundo.
        /// </summary>
        /// <param name="dateTime">DateTime local como UTC or Unspecified DateTimeKind.</param>
        /// <param name="timezone">Nombre de la zona horaria (en formato TZDB).</param>
        /// <returns>UTC DateTime como UTC DateTimeKind.</returns>
        public static DateTime InZone(this DateTime dateTime, string timezone)
        {
            if (dateTime.Kind == DateTimeKind.Local)
                throw new ArgumentException("Expected non-local kind of DateTime");

            var zone = GetTimeZone(timezone);
            LocalDateTime asLocal = dateTime.ToLocalDateTime();
            ZonedDateTime asZoned = asLocal.InZoneLeniently(zone);
            Instant instant = asZoned.ToInstant();
            ZonedDateTime asZonedInUtc = instant.InUtc();
            DateTime utc = asZonedInUtc.ToDateTimeUtc();

            return utc;
        }

        public static DateTimeZone GetTimeZone(string id)
        {
            DateTimeZone timezone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(id);

            if (timezone == null)
            {
                var instant = Instant.FromDateTimeUtc(DateTime.UtcNow);
                var source = DateTimeZoneProviders.Tzdb;
                id = source.Ids.FirstOrDefault(i => source[i].GetZoneInterval(instant).Name == id);
                timezone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(id);
            }

            return timezone;
        }

        public static string GetDateTimeISO(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        public static string GetDateTimeISO(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return GetDateTimeISO(dateTime.Value);
            }

            return string.Empty;
        }

        public static string GetDateISO(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static string GetDateISO(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return GetDateISO(dateTime.Value);
            }

            return string.Empty;
        }

        public static string GetTimeISO(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        public static string GetTimeISO(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return GetTimeISO(dateTime.Value);
            }

            return string.Empty;

        }

        public static string GetDateTimeForLogChange(this DateTime dateTime)
        {
            return string.Format("{{{{(DT){0}}}}}", dateTime.GetDateTimeISO());
        }

        public static string GetDateTimeForLogChange(this DateTime? dateTime)
        {
            return string.Format("{{{{(DT){0}}}}}", dateTime.GetDateTimeISO());
        }

        public static string GetDateISOForLogChange(this DateTime dateTime)
        {
            return string.Format("{{{{(D){0}}}}}", dateTime.GetDateISO());
        }

        public static string GetDateISOForLogChange(this DateTime? dateTime)
        {
            return string.Format("{{{{(D){0}}}}}", dateTime.GetDateISO());
        }

        public static string GetTimeISOForLogChange(this DateTime dateTime)
        {
            return string.Format("{{{{(T){0}}}}}", dateTime.GetTimeISO());
        }

        public static string GetTimeISOForLogChange(this DateTime? dateTime)
        {
            return string.Format("{{{{(T){0}}}}}", dateTime.GetTimeISO());
        }
    }
}
