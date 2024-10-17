using System;

namespace DateTimeExtensionsLibrary
{
    public static partial class DateTimeExtensions
    {

        /// <summary>
        /// Converts the DateTime to an ISO 8601 string.
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <returns>An ISO 8601 formatted string representing the date.</returns>
        public static string ToIso8601String(this DateTime date)
        {
            return date.ToString("o");
        }

        /// <summary>
        /// Converts the DateTime to a short date string.
        /// </summary>
        /// <param name="date">The DateTime to convert.</param>
        /// <returns>A short date string.</returns>
        public static string ToShortDateString(this DateTime date)
        {
            return date.ToString("d");
        }

        /// <summary>
        /// Converts the DateTime to a long date string.
        /// </summary>
        /// <param name="date">The DateTime to convert.</param>
        /// <returns>A long date string.</returns>
        public static string ToLongDateString(this DateTime date)
        {
            return date.ToString("D");
        }

        /// <summary>
        /// Converts the DateTime to a short time string.
        /// </summary>
        /// <param name="date">The DateTime to convert.</param>
        /// <returns>A short time string.</returns>
        public static string ToShortTimeString(this DateTime date)
        {
            return date.ToString("t");
        }

        /// <summary>
        /// Converts the DateTime to a long time string.
        /// </summary>
        /// <param name="date">The DateTime to convert.</param>
        /// <returns>A long time string.</returns>
        public static string ToLongTimeString(this DateTime date)
        {
            return date.ToString("T");
        }

        /// <summary>
        /// Converts the DateTime to a custom formatted string.
        /// </summary>
        /// <param name="date">The DateTime to convert.</param>
        /// <param name="format">The custom format string.</param>
        /// <returns>A custom formatted date string.</returns>
        public static string ToCustomFormat(this DateTime date, string format)
        {
            return date.ToString(format);
        }

        /// <summary>
        /// Converts the DateTime to an ISO 8601 formatted string.
        /// </summary>
        /// <param name="date">The DateTime to convert.</param>
        /// <returns>An ISO 8601 formatted date string.</returns>
        public static string ToISO8601(this DateTime date)
        {
            return date.ToString("o");
        }

        /// <summary>
        /// Converts the DateTime to a day of the week string.
        /// </summary>
        /// <param name="date">The DateTime to convert.</param>
        /// <returns>The day of the week as a string.</returns>
        public static string ToDayOfWeekString(this DateTime date)
        {
            return date.DayOfWeek.ToString();
        }

        /// <summary>
        /// Converts the DateTime to the name of the month.
        /// </summary>
        /// <param name="date">The DateTime to convert.</param>
        /// <returns>The name of the month.</returns>
        public static string ToMonthName(this DateTime date)
        {
            return date.ToString("MMMM");
        }

        /// <summary>
        /// Converts the DateTime to a friendly date string (e.g., "today", "tomorrow").
        /// </summary>
        /// <param name="date">The DateTime to convert.</param>
        /// <returns>A friendly date string.</returns>
        public static string ToFriendlyDateString(this DateTime date)
        {
            var today = DateTime.Today;
            if (date.Date == today)
                return "today";
            if (date.Date == today.AddDays(1))
                return "tomorrow";
            if (date.Date == today.AddDays(-1))
                return "yesterday";
            return date.ToString("D");
        }

        /// <summary>
        /// Converts the DateTime to an ordinal date string (e.g., "1st", "2nd").
        /// </summary>
        /// <param name="date">The DateTime to convert.</param>
        /// <returns>An ordinal date string.</returns>
        public static string ToOrdinalDateString(this DateTime date)
        {
            int day = date.Day;
            string suffix = day switch
            {
                1 or 21 or 31 => "st",
                2 or 22 => "nd",
                3 or 23 => "rd",
                _ => "th"
            };
            return $"{date:MMMM} {day}{suffix}, {date:yyyy}";
        }


    }
}
