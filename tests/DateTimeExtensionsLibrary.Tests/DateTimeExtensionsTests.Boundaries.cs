using System;
using DateTimeExtensionsLibrary;
using Xunit;

namespace DateTimeExtensionsLibrary.Tests
{
    public class DateTimeExtensionsBoundariesTests
    {
        [Fact]
        public void StartOfDay_ShouldSetZeroTime_PreservingKind()
        {
            var date = new DateTime(2026, 9, 28, 15, 34, 22, 500, DateTimeKind.Utc);
            var start = date.StartOfDay();

            Assert.Equal(new DateTime(2026, 9, 28, 0, 0, 0, 0, DateTimeKind.Utc), start);
            Assert.Equal(DateTimeKind.Utc, start.Kind);
        }

        [Fact]
        public void EndOfDay_ShouldSetEndOfDayTime_PreservingKind()
        {
            var date = new DateTime(2026, 9, 28, 10, 15, 0, DateTimeKind.Local);
            var end = date.EndOfDay();

            Assert.Equal(new DateTime(2026, 9, 28, 23, 59, 59, 999, DateTimeKind.Local), end);
            Assert.Equal(DateTimeKind.Local, end.Kind);
        }

        [Fact]
        public void StartOfYear_And_EndOfYear_ShouldSetCorrectBounds()
        {
            var date = new DateTime(2024, 6, 15, 12, 0, 0, DateTimeKind.Utc);

            var start = date.StartOfYear();
            var end = date.EndOfYear();

            Assert.Equal(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), start);
            Assert.Equal(new DateTime(2024, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc), end);
        }

        [Theory]
        [InlineData("1990-05-15", "2026-05-14", 35)]
        [InlineData("1990-05-15", "2026-05-15", 36)]
        [InlineData("1990-05-15", "2026-05-16", 36)]
        [InlineData("2000-02-29", "2001-02-28", 0)]
        [InlineData("2000-02-29", "2001-03-01", 1)]
        public void CalculateAge_ShouldReturnExactAge(string birthDateString, string asOfDateString, int expectedAge)
        {
            var birthDate = DateTime.Parse(birthDateString);
            var asOfDate = DateTime.Parse(asOfDateString);

            int age = birthDate.CalculateAge(asOfDate);
            Assert.Equal(expectedAge, age);
        }

        [Fact]
        public void CalculateAge_WhenAsOfPrecedesBirth_ShouldThrowArgumentException()
        {
            var birthDate = new DateTime(2026, 1, 1);
            var asOfDate = new DateTime(2025, 1, 1);

            Assert.Throws<ArgumentException>(() => birthDate.CalculateAge(asOfDate));
        }

#if NET8_0_OR_GREATER
        [Fact]
        public void DateOnly_And_TimeOnly_Interop_ShouldConvertAccurately()
        {
            var dateTime = new DateTime(2026, 9, 28, 14, 30, 45, DateTimeKind.Utc);

            DateOnly dateOnly = dateTime.ToDateOnly();
            TimeOnly timeOnly = dateTime.ToTimeOnly();

            Assert.Equal(new DateOnly(2026, 9, 28), dateOnly);
            Assert.Equal(new TimeOnly(14, 30, 45), timeOnly);

            DateTime combined = dateOnly.ToDateTime(timeOnly, DateTimeKind.Utc);
            Assert.Equal(dateTime, combined);
            Assert.Equal(DateTimeKind.Utc, combined.Kind);
        }

        [Fact]
        public void DateOnly_CalculateAge_ShouldMatchDateTimeLogic()
        {
            var birth = new DateOnly(1995, 10, 20);
            var asOf = new DateOnly(2026, 10, 19);

            Assert.Equal(30, birth.CalculateAge(asOf));
            Assert.Equal(31, birth.CalculateAge(new DateOnly(2026, 10, 20)));
        }
#endif
    }
}
