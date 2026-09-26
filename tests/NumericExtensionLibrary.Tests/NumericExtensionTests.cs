using System;
using System.Collections.Generic;
using NumericExtensionLibrary;
using Xunit;

namespace NumericExtensionLibrary.Tests;

public class NumericExtensionTests
{
    [Fact]
    public void DigitSum_IntAndLong_ShouldSumDigitsCorrectly()
    {
        Assert.Equal(6, 123.DigitSum());
        Assert.Equal(15, 555.DigitSum());
        Assert.Equal(1, 1000.DigitSum());
        Assert.Equal(18, 99000000000L.DigitSum());
    }

    [Fact]
    public void GreatestCommonMultiple_ShouldHandleZeroSafely()
    {
        Assert.Equal(0, 0.GreatestCommonMultiple(10));
        Assert.Equal(0, 10.GreatestCommonMultiple(0));
        Assert.Equal(12, 4.GreatestCommonMultiple(6));
    }

    [Fact]
    public void GreatestCommonDivisor_ShouldCalculateGcd()
    {
        Assert.Equal(2, 4.GreatestCommonDivisor(6));
        Assert.Equal(5, 10.GreatestCommonDivisor(15));
    }

    [Fact]
    public void CheckExtensions_IsPrime_IsEven_IsOdd_IsPerfectSquare()
    {
        Assert.True(7.IsPrime());
        Assert.False(4.IsPrime());
        Assert.False(1.IsPrime());

        Assert.True(4.IsEven());
        Assert.False(5.IsEven());

        Assert.True(5.IsOdd());
        Assert.False(4.IsOdd());

        Assert.True(16.IsPerfectSquare());
        Assert.False(15.IsPerfectSquare());

        Assert.True(10.IsMultipleOf(5));
        Assert.False(10.IsMultipleOf(3));
        Assert.False(10.IsMultipleOf(0));
    }

    [Fact]
    public void Factorial_ShouldCalculateCorrectly()
    {
        Assert.Equal(1, 0.Factorial());
        Assert.Equal(1, 1.Factorial());
        Assert.Equal(120, 5.Factorial());
        Assert.Equal(2432902008176640000L, 20.Factorial());
        Assert.Throws<ArgumentOutOfRangeException>(() => (-1).Factorial());
        Assert.Throws<ArgumentOutOfRangeException>(() => 21.Factorial());
    }

    [Fact]
    public void Conversion_BinaryAndHex()
    {
        Assert.Equal("1010", 10.ToBinaryString());
        Assert.Equal("FF", 255.ToHexString());
    }

    [Fact]
    public void Decimal_And_Double_Calculations()
    {
        decimal valDec = 200M;
        Assert.Equal(20M, valDec.Percentage(10M));
        Assert.True(valDec.IsEven());
        Assert.False(valDec.IsOdd());
        Assert.Equal(150M, valDec.Subtract(50M));

        double valDouble = 200.0;
        Assert.Equal(20.0, valDouble.Percentage(10.0));
        Assert.True(valDouble.IsEven());
        Assert.False(valDouble.IsOdd());
        Assert.Equal(150.0, valDouble.Subtract(50.0));
    }

    [Fact]
    public void WeightedAverage_ShouldCalculateCorrectly()
    {
        var values = new List<decimal> { 10M, 20M };
        var weights = new List<decimal> { 1M, 3M };
        Assert.Equal(17.5M, values.WeightedAverage(weights));

        var dValues = new List<double> { 10.0, 20.0 };
        var dWeights = new List<double> { 1.0, 3.0 };
        Assert.Equal(17.5, dValues.WeightedAverage(dWeights));
    }
}
