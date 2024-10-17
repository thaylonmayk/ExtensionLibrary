using System;
using System.Collections.Generic;

namespace DateTimeExtensionsLibrary
{
    public static partial class DateTimeExtensions
    {
        /// <summary>
        /// Splits the date range from the start date to the end date into chunks of specified days.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <param name="days">The number of days in each chunk.</param>
        /// <returns>A list of tuples, each containing the start and end date of a chunk.</returns>
        public static List<(DateTime, DateTime)> Chunks(this DateTime startDate, DateTime endDate, int days)
        {
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var chunks = new List<(DateTime, DateTime)>();
            var currentDate = startDate;
            while (currentDate <= endDate)
            {
                var chunkStartDate = currentDate;
                var chunkEndDate = currentDate.AddDays(days).AddHours(23).AddMinutes(59).AddSeconds(59);
                if (chunkEndDate > endDate) chunkEndDate = endDate;
                chunks.Add((chunkStartDate, chunkEndDate));
                currentDate = chunkEndDate.AddSeconds(1);
            }
            return chunks;
        }

    }
}
