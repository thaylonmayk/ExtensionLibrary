using System;
using System.Collections.Generic;
using System.Linq;

namespace DateTimeExtensionsLibrary
{
    /// <summary>
    /// Contract for providing holiday dates to corporate calendar calculations.
    /// </summary>
    public interface IHolidayProvider
    {
        /// <summary>
        /// Determines whether the specified date is a corporate, bank, or national holiday.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <returns>True if the date is a holiday; otherwise, false.</returns>
        bool IsHoliday(DateTime date);
    }

    /// <summary>
    /// Default null object implementation of IHolidayProvider that treats no dates as holidays.
    /// </summary>
    public sealed class NullHolidayProvider : IHolidayProvider
    {
        /// <summary>
        /// Singleton instance of NullHolidayProvider.
        /// </summary>
        public static readonly NullHolidayProvider Instance = new NullHolidayProvider();

        public bool IsHoliday(DateTime date) => false;
    }

    public static partial class DateTimeExtensions
    {
        /// <summary>
        /// Calculates the number of business days between two dates, excluding weekends and holidays from the provider.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="holidayProvider">The holiday provider to evaluate non-working days.</param>
        /// <returns>The number of business days between the two dates.</returns>
        public static int BusinessDaysBetween(this DateTime startDate, DateTime endDate, IHolidayProvider holidayProvider)
        {
            if (holidayProvider is null || holidayProvider is NullHolidayProvider)
            {
                return startDate.BusinessDaysBetween(endDate);
            }

            return startDate.BusinessDaysBetween(endDate, holidayProvider.IsHoliday);
        }

        /// <summary>
        /// Calculates the number of business days between two dates, excluding weekends and explicit holidays.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="holidays">The collection of holiday dates.</param>
        /// <returns>The number of business days between the two dates.</returns>
        public static int BusinessDaysBetween(this DateTime startDate, DateTime endDate, IEnumerable<DateTime> holidays)
        {
            if (holidays is null)
            {
                return startDate.BusinessDaysBetween(endDate);
            }

            var holidaySet = new HashSet<DateTime>(holidays.Select(h => h.Date));
            return startDate.BusinessDaysBetween(endDate, holidaySet.Contains);
        }

        /// <summary>
        /// Calculates the number of business days between two dates, excluding weekends and dates matching the holiday predicate.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="isHoliday">The predicate to determine whether a date is a holiday.</param>
        /// <returns>The number of business days between the two dates.</returns>
        public static int BusinessDaysBetween(this DateTime startDate, DateTime endDate, Func<DateTime, bool> isHoliday)
        {
            var start = startDate.Date;
            var end = endDate.Date;

            if (start > end) return 0;

            if (isHoliday is null)
            {
                return startDate.BusinessDaysBetween(endDate);
            }

            int businessDays = 0;
            for (var current = start; current <= end; current = current.AddDays(1))
            {
                if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday && !isHoliday(current))
                {
                    businessDays++;
                }
            }

            return businessDays;
        }

        /// <summary>
        /// Calculates the number of business days until the specified future date, excluding holidays from the provider.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="futureDate">The future date.</param>
        /// <param name="holidayProvider">The holiday provider.</param>
        /// <returns>The number of business days until the future date.</returns>
        public static int BusinessDaysUntil(this DateTime date, DateTime futureDate, IHolidayProvider holidayProvider) =>
            date.BusinessDaysBetween(futureDate, holidayProvider);

        /// <summary>
        /// Calculates the number of business days until the specified future date, excluding explicit holidays.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="futureDate">The future date.</param>
        /// <param name="holidays">The collection of holidays.</param>
        /// <returns>The number of business days until the future date.</returns>
        public static int BusinessDaysUntil(this DateTime date, DateTime futureDate, IEnumerable<DateTime> holidays) =>
            date.BusinessDaysBetween(futureDate, holidays);

        /// <summary>
        /// Calculates the number of business days until the specified future date, excluding dates matching the holiday predicate.
        /// </summary>
        /// <param name="date">The starting date.</param>
        /// <param name="futureDate">The future date.</param>
        /// <param name="isHoliday">The predicate to determine holidays.</param>
        /// <returns>The number of business days until the future date.</returns>
        public static int BusinessDaysUntil(this DateTime date, DateTime futureDate, Func<DateTime, bool> isHoliday) =>
            date.BusinessDaysBetween(futureDate, isHoliday);
    }
}
