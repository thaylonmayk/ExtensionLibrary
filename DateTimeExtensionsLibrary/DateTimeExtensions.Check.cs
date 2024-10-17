using System;

namespace DateTimeExtensionsLibrary
{
    public static partial class DateTimeExtensions
    { 
        /// <summary>
        /// Checks if the DateTime is a weekend.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns>True if the date is a weekend; otherwise, false.</returns>
        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Checks if the year of the DateTime is a leap year.
        /// </summary>
        /// <param name="date">The date to check the year for.</param>
        /// <returns>True if the year is a leap year; otherwise, false.</returns>
        public static bool IsLeapYear(this DateTime date)
        {
            return DateTime.IsLeapYear(date.Year);
        }

        /// <summary>
        /// Checks if the DateTime is a business day (Monday to Friday).
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns>True if the date is a business day; otherwise, false.</returns>
        public static bool IsBusinessDay(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

    }
}
