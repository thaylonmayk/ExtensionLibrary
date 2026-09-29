using System;
using DateTimeExtensionsLibrary;
using Xunit;

namespace DateTimeExtensionsLibrary.Tests;

public class DateTimeExtensionTests
{
    [Fact]
    public void BusinessDaysBetween_ShouldCalculateCorrectly()
    {
        var start = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 9, 7, 0, 0, 0, DateTimeKind.Utc);

        int businessDays = start.BusinessDaysBetween(end);
        Assert.Equal(5, businessDays);
    }

    [Fact]
    public void DaysUntil_ShouldCalculateRemainingDays()
    {
        var today = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        var future = new DateTime(2026, 9, 10, 0, 0, 0, DateTimeKind.Utc);

        Assert.Equal(9, today.DaysUntil(future));
    }

    [Fact]
    public void IsWeekend_And_IsBusinessDay_ShouldIdentifyCorrectly()
    {
        var saturday = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc);
        var sunday = new DateTime(2026, 9, 6, 0, 0, 0, DateTimeKind.Utc);
        var monday = new DateTime(2026, 9, 7, 0, 0, 0, DateTimeKind.Utc);

        Assert.True(saturday.IsWeekend());
        Assert.True(sunday.IsWeekend());
        Assert.False(monday.IsWeekend());

        Assert.False(saturday.IsBusinessDay());
        Assert.True(monday.IsBusinessDay());
    }

    [Fact]
    public void IsLeapYear_ShouldIdentifyLeapYears()
    {
        var leap = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var nonLeap = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        Assert.True(leap.IsLeapYear());
        Assert.False(nonLeap.IsLeapYear());
    }

    [Fact]
    public void Chunks_ShouldSplitRangeCorrectly()
    {
        var start = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 9, 15, 0, 0, 0, DateTimeKind.Utc);

        var chunks = start.Chunks(end, days: 5);
        Assert.NotEmpty(chunks);
        Assert.Equal(start.Date, chunks[0].Item1.Date);
    }

    [Fact]
    public void BusinessDaysBetween_WithExplicitHolidays_ShouldExcludeHolidays()
    {
        var holidayMonday = new DateTime(2026, 9, 7);
        var endFriday = new DateTime(2026, 9, 11);

        var holidays = new[] { holidayMonday };
        var businessDays = holidayMonday.BusinessDaysBetween(endFriday, holidays);

        Assert.Equal(4, businessDays);
    }

    [Fact]
    public void BusinessDaysBetween_WithHolidayProvider_ShouldDeductWorkingHolidays()
    {
        var startTuesday = new DateTime(2026, 9, 1);
        var endFriday = new DateTime(2026, 9, 4);

        var provider = new TestCorporateHolidayProvider(new[] { new DateTime(2026, 9, 2) });
        var businessDays = startTuesday.BusinessDaysBetween(endFriday, provider);

        Assert.Equal(3, businessDays);
    }

    [Fact]
    public void ToUnixTimestamp_WithUnspecifiedKind_ShouldNormalizeToUtcConsistently()
    {
        var unspecified = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
        var timestamp = unspecified.ToUnixTimestamp();

        Assert.Equal(0, timestamp);
    }

    private sealed class TestCorporateHolidayProvider : IHolidayProvider
    {
        private readonly HashSet<DateTime> _holidays;

        public TestCorporateHolidayProvider(IEnumerable<DateTime> holidays)
        {
            _holidays = new HashSet<DateTime>(holidays);
        }

        public bool IsHoliday(DateTime date) => _holidays.Contains(date.Date);
    }
}