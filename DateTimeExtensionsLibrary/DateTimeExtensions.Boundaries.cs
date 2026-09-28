using System;

namespace DateTimeExtensionsLibrary
{
    public static partial class DateTimeExtensions
    {
        /// <summary>
        /// Returns a new DateTime representing the start of the day at 00:00:00.000,
        /// preserving the original DateTimeKind.
        /// </summary>
        /// <param name="dateTime">The source date.</param>
        /// <returns>Start of the day with identical DateTimeKind.</returns>
        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, dateTime.Kind);
        }

        /// <summary>
        /// Returns a new DateTime representing the end of the day at 23:59:59.999,
        /// preserving the original DateTimeKind.
        /// </summary>
        /// <param name="dateTime">The source date.</param>
        /// <returns>End of the day with identical DateTimeKind.</returns>
        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999, dateTime.Kind);
        }

        /// <summary>
        /// Returns a new DateTime representing the first moment of the year at 00:00:00.000,
        /// preserving the original DateTimeKind.
        /// </summary>
        /// <param name="dateTime">The source date.</param>
        /// <returns>Start of the year with identical DateTimeKind.</returns>
        public static DateTime StartOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1, 0, 0, 0, 0, dateTime.Kind);
        }

        /// <summary>
        /// Returns a new DateTime representing the last moment of the year at 23:59:59.999,
        /// preserving the original DateTimeKind.
        /// </summary>
        /// <param name="dateTime">The source date.</param>
        /// <returns>End of the year with identical DateTimeKind.</returns>
        public static DateTime EndOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 12, 31, 23, 59, 59, 999, dateTime.Kind);
        }

        /// <summary>
        /// Calculates the age in completed years from the birth date relative to an evaluation date.
        /// </summary>
        /// <param name="birthDate">The date of birth.</param>
        /// <param name="asOfDate">The date to evaluate the age at.</param>
        /// <returns>The age in completed whole years.</returns>
        /// <exception cref="ArgumentException">Thrown when evaluation date precedes birth date.</exception>
        public static int CalculateAge(this DateTime birthDate, DateTime asOfDate)
        {
            if (asOfDate < birthDate)
            {
                throw new ArgumentException("Evaluation date cannot be earlier than birth date.", nameof(asOfDate));
            }

            int age = asOfDate.Year - birthDate.Year;
            if (HasNotReachedBirthdayThisYear(birthDate, asOfDate))
            {
                age--;
            }

            return age;
        }

        private static bool HasNotReachedBirthdayThisYear(DateTime birthDate, DateTime asOfDate)
        {
            return asOfDate.Month < birthDate.Month || (asOfDate.Month == birthDate.Month && asOfDate.Day < birthDate.Day);
        }

#if NET8_0_OR_GREATER
        /// <summary>
        /// Converts a DateTime instance to a modern DateOnly instance.
        /// </summary>
        /// <param name="dateTime">The source date.</param>
        /// <returns>The corresponding DateOnly representation.</returns>
        public static DateOnly ToDateOnly(this DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime);
        }

        /// <summary>
        /// Converts a DateTime instance to a modern TimeOnly instance.
        /// </summary>
        /// <param name="dateTime">The source date.</param>
        /// <returns>The corresponding TimeOnly representation.</returns>
        public static TimeOnly ToTimeOnly(this DateTime dateTime)
        {
            return TimeOnly.FromDateTime(dateTime);
        }

        /// <summary>
        /// Combines a DateOnly and a TimeOnly into a complete DateTime instance.
        /// </summary>
        /// <param name="dateOnly">The date part.</param>
        /// <param name="timeOnly">The time part.</param>
        /// <param name="kind">The DateTimeKind to apply.</param>
        /// <returns>The combined DateTime representation.</returns>
        public static DateTime ToDateTime(this DateOnly dateOnly, TimeOnly timeOnly, DateTimeKind kind = DateTimeKind.Unspecified)
        {
            return dateOnly.ToDateTime(timeOnly, kind);
        }

        /// <summary>
        /// Calculates the age in completed years from the birth date relative to an evaluation date.
        /// </summary>
        /// <param name="birthDate">The date of birth.</param>
        /// <param name="asOfDate">The date to evaluate the age at.</param>
        /// <returns>The age in completed whole years.</returns>
        /// <exception cref="ArgumentException">Thrown when evaluation date precedes birth date.</exception>
        public static int CalculateAge(this DateOnly birthDate, DateOnly asOfDate)
        {
            if (asOfDate < birthDate)
            {
                throw new ArgumentException("Evaluation date cannot be earlier than birth date.", nameof(asOfDate));
            }

            int age = asOfDate.Year - birthDate.Year;
            if (asOfDate.Month < birthDate.Month || (asOfDate.Month == birthDate.Month && asOfDate.Day < birthDate.Day))
            {
                age--;
            }

            return age;
        }
#endif
    }
}
