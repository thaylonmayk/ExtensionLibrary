using System;

namespace DateTimeExtensionsLibrary
{
    public static partial class DateTimeExtensions
    {
        /// <summary>
        /// Adds the specified number of business days to the DateTime.
        /// </summary>
        /// <param name="date">The date to add business days to.</param>
        /// <param name="days">The number of business days to add.</param>
        /// <returns>The DateTime with the business days added.</returns>
        public static DateTime AddBusinessDays(this DateTime date, int days)
        {
            int addedDays = 0;
            while (addedDays < days)
            {
                date = date.AddDays(1);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    addedDays++;
                }
            }
            return date;
        }

        /// <summary>
        /// Gets the start date of the week for the specified DateTime.
        /// </summary>
        /// <param name="date">The date to get the start of the week for.</param>
        /// <returns>The start date of the week.</returns>
        public static DateTime StartOfWeek(this DateTime date)
        {
            int diff = date.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0)
            {
                diff += 7;
            }
            return date.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Gets the end date of the week for the specified DateTime.
        /// </summary>
        /// <param name="date">The date to get the end of the week for.</param>
        /// <returns>The end date of the week.</returns>
        public static DateTime EndOfWeek(this DateTime date)
        {
            return date.StartOfWeek().AddDays(6);
        }

        /// <summary>
        /// Gets the start date of the month for the specified DateTime.
        /// </summary>
        /// <param name="date">The date to get the start of the month for.</param>
        /// <returns>The start date of the month.</returns>
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Gets the end date of the month for the specified DateTime.
        /// </summary>
        /// <param name="date">The date to get the end of the month for.</param>
        /// <returns>The end date of the month.</returns>
        public static DateTime EndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        /// <summary>
        /// Gets the next occurrence of the specified day of the week.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="dayOfWeek">The day of the week to find.</param>
        /// <returns>The next occurrence of the specified day of the week.</returns>
        public static DateTime NextWeekday(this DateTime date, DayOfWeek dayOfWeek)
        {
            int daysToAdd = ((int)dayOfWeek - (int)date.DayOfWeek + 7) % 7;
            return date.AddDays(daysToAdd == 0 ? 7 : daysToAdd);
        }

        /// <summary>
        /// Gets the previous occurrence of the specified day of the week.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="dayOfWeek">The day of the week to find.</param>
        /// <returns>The previous occurrence of the specified day of the week.</returns>
        public static DateTime PreviousWeekday(this DateTime date, DayOfWeek dayOfWeek)
        {
            int daysToSubtract = ((int)date.DayOfWeek - (int)dayOfWeek + 7) % 7;
            return date.AddDays(daysToSubtract == 0 ? -7 : -daysToSubtract);
        }

        /// <summary>
        /// Converts a Unix timestamp to a DateTime.
        /// </summary>
        /// <param name="timestamp">The Unix timestamp to convert.</param>
        /// <returns>The DateTime representing the Unix timestamp.</returns>
        public static DateTime FromUnixTimestamp(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
        }

        /// <summary>
        /// Gets the next business day after the specified DateTime.
        /// </summary>
        /// <param name="date">The date to find the next business day for.</param>
        /// <returns>The next business day.</returns>
        public static DateTime NextBusinessDay(this DateTime date)
        {
            do
            {
                date = date.AddDays(1);
            } while (!date.IsBusinessDay());

            return date;
        }

    }
}
