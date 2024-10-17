using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Provides utility methods for working with Double numbers.
/// </summary>
public static partial class NumericExtension
{
    /// <summary>
    /// Calculates the percentage of a number.
    /// </summary>
    /// <param name="number">The base number.</param>
    /// <param name="percentage">The percentage to calculate.</param>
    /// <returns>The calculated percentage.</returns>
    public static double Percentage(this double number, double percentage)
    {
        return (number * percentage) / 100.0;
    }

    /// <summary>
    /// Calculates the weighted average of a list of numbers with corresponding weights.
    /// </summary>
    /// <param name="numbers">The list of numbers.</param>
    /// <param name="weights">The list of weights.</param>
    /// <returns>The weighted average.</returns>
    public static double WeightedAverage(this List<double> numbers, List<double> weights)
    {
        if (numbers.Count != weights.Count) throw new ArgumentException("Numbers and weights must have the same length.");

        double weightedSum = 0;
        double weightSum = 0;

        for (int i = 0; i < numbers.Count; i++)
        {
            weightedSum += numbers[i] * weights[i];
            weightSum += weights[i];
        }

        return weightedSum / weightSum;
    }

    /// <summary>
    /// Checks if a number is even.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>True if the number is even; otherwise, false.</returns>
    public static bool IsEven(this double number)
    {
        return number % 2 == 0;
    }

    /// <summary>
    /// Checks if a number is odd.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>True if the number is odd; otherwise, false.</returns>
    public static bool IsOdd(this double number)
    {
        return number % 2 != 0;
    }

    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    /// <param name="degrees">The degrees to convert.</param>
    /// <returns>The equivalent in radians.</returns>
    public static double ToRadians(this double degrees)
    {
        return degrees * (Math.PI / 180.0);
    }

    /// <summary>
    /// Converts radians to degrees.
    /// </summary>
    /// <param name="radians">The radians to convert.</param>
    /// <returns>The equivalent in degrees.</returns>
    public static double ToDegrees(this double radians)
    {
        return radians * (180.0 / Math.PI);
    }

    /// <summary>
    /// Calculates the sum of the digits of a number.
    /// </summary>
    /// <param name="number">The number to calculate the digit sum of.</param>
    /// <returns>The sum of the digits of the number.</returns>
    public static int DigitSum(this double number)
    {
        return number.ToString().Where(char.IsDigit).Sum(c => c - '0');
    }

    /// <summary>
    /// Reverses the digits of a number.
    /// </summary>
    /// <param name="number">The number to reverse the digits of.</param>
    /// <returns>The number with its digits reversed.</returns>
    public static double ReverseDigits(this double number)
    {
        var reversed = new string(number.ToString().Reverse().ToArray());
        return double.Parse(reversed);
    }

    /// <summary>
    /// Subtracts a double number from another.
    /// </summary>
    /// <param name="number">The base number.</param>
    /// <param name="subtrahend">The number to subtract.</param>
    /// <returns>The result of the subtraction.</returns>
    public static double Subtract(this double number, double subtrahend)
    {
        return number - subtrahend;
    }
}
