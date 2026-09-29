using System;
using NumericExtensionLibrary;
using Xunit;

namespace NumericExtensionLibrary.Tests
{
    public class NumericExtensionTestsSafeMath
    {
        [Fact]
        public void SafeDivide_Decimal_DivisorIsZero_ShouldReturnFallback()
        {
            Assert.Equal(0m, 100m.SafeDivide(0m));
            Assert.Equal(-1m, 100m.SafeDivide(0m, fallback: -1m));
            Assert.Equal(50m, 100m.SafeDivide(2m));
            Assert.Equal(-25m, (-50m).SafeDivide(2m));
        }

        [Fact]
        public void SafeDivide_Double_DivisorIsZeroOrInvalid_ShouldReturnFallback()
        {
            Assert.Equal(0.0, 100.0.SafeDivide(0.0));
            Assert.Equal(-1.0, 100.0.SafeDivide(double.NaN, fallback: -1.0));
            Assert.Equal(-1.0, 100.0.SafeDivide(double.PositiveInfinity, fallback: -1.0));
            Assert.Equal(50.0, 100.0.SafeDivide(2.0));
        }

        [Fact]
        public void CalculatePercentageOf_DecimalAndDouble_ShouldCalculateCorrectly()
        {
            Assert.Equal(25m, 50m.CalculatePercentageOf(200m));
            Assert.Equal(0m, 50m.CalculatePercentageOf(0m));
            Assert.Equal(100m, 200m.CalculatePercentageOf(200m));

            Assert.Equal(25.0, 50.0.CalculatePercentageOf(200.0));
            Assert.Equal(0.0, 50.0.CalculatePercentageOf(0.0));
            Assert.Equal(0.0, 50.0.CalculatePercentageOf(double.NaN));
        }

        [Fact]
        public void RoundFinancial_BankersRounding_ShouldRoundToNearestEvenNumber()
        {
            Assert.Equal(2.50m, 2.505m.RoundFinancial(2));
            Assert.Equal(2.52m, 2.515m.RoundFinancial(2));
            Assert.Equal(2.52m, 2.525m.RoundFinancial(2));
            Assert.Equal(2.54m, 2.535m.RoundFinancial(2));
        }

        [Fact]
        public void RoundFinancial_InvalidDecimals_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 10.5m.RoundFinancial(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => 10.5m.RoundFinancial(29));
        }

        [Fact]
        public void IsBetween_ComparableTypes_ShouldEvaluateInclusively()
        {
            Assert.True(15.IsBetween(10, 20));
            Assert.True(10.IsBetween(10, 20));
            Assert.True(20.IsBetween(10, 20));
            Assert.False(9.IsBetween(10, 20));
            Assert.False(21.IsBetween(10, 20));
        }

        [Fact]
        public void IsBetween_WithInvertedBounds_ShouldEvaluateInclusively()
        {
            Assert.True(15.IsBetween(20, 10));
            Assert.False(5.IsBetween(20, 10));
        }

        [Fact]
        public void IsBetween_WithDecimalAndDateTime_ShouldEvaluateInclusively()
        {
            Assert.True(15.5m.IsBetween(10.0m, 20.0m));

            var now = new DateTime(2026, 9, 21, 12, 0, 0, DateTimeKind.Utc);
            var start = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(2026, 9, 30, 23, 59, 59, DateTimeKind.Utc);
            Assert.True(now.IsBetween(start, end));
        }

        [Fact]
        public void IsBetween_NullParameters_ShouldThrowArgumentNullException()
        {
            string? value = null;
            Assert.Throws<ArgumentNullException>(() => value!.IsBetween("a", "z"));

            Assert.Throws<ArgumentNullException>(() => "m".IsBetween(null!, "z"));
            Assert.Throws<ArgumentNullException>(() => "m".IsBetween("a", null!));
        }
    }
}
