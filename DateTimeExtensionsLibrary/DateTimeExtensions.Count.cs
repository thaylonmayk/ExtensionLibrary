using System;

namespace DateTimeExtensionsLibrary
{
    public static partial class DateTimeExtensions
    {
        /// <summary>
        /// Gets the number of days in the month of the specified DateTime.
        /// </summary>
        /// <param name="date">The date to get the number of days in the month for.</param>
        /// <returns>The number of days in the month.</returns>
        public static int DaysInMonth(this DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }

        /// <summary>
        /// Calculates the age based on the birth date.
        /// </summary>
        /// <param name="birthDate">The birth date to calculate the age from.</param>
        /// <returns>The age in years.</returns>
        public static int Age(this DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        /// <summary>
        /// Converts the DateTime to a Unix timestamp.
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <returns>The Unix timestamp representing the date.</returns>
        public static long ToUnixTimestamp(this DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
        }

        /// <summary>
        /// Calculates the number of business days between two dates.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The number of business days between the two dates.</returns>
        public static int BusinessDaysBetween(this DateTime startDate, DateTime endDate)
        {
            int businessDays = 0;
            DateTime currentDate = startDate;

            while (currentDate <= endDate)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDays++;
                }
                currentDate = currentDate.AddDays(1);
            }

            return businessDays;
        }

        /// <summary>
        /// Calculates the number of days until the specified future date.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="futureDate">The future date to calculate the days until.</param>
        /// <returns>The number of days until the future date.</returns>
        public static int DaysUntil(this DateTime date, DateTime futureDate)
        {
            return (futureDate.Date - date.Date).Days;
        }

        /// <summary>
        /// Calculates the number of business days until the specified future date.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="futureDate">The future date to calculate the business days until.</param>
        /// <returns>The number of business days until the future date.</returns>
        public static int BusinessDaysUntil(this DateTime date, DateTime futureDate)
        {
            int businessDays = 0;
            DateTime currentDate = date;

            while (currentDate <= futureDate)
            {
                if (currentDate.IsBusinessDay())
                {
                    businessDays++;
                }
                currentDate = currentDate.AddDays(1);
            }

            return businessDays;
        }

    }
}
