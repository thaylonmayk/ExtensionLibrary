using System;

namespace NumericExtensionLibrary
{
    public static partial class NumericExtension
    {


        /// <summary>
        /// Calculates the sum of the digits of a number.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The sum of the digits of the number.</returns>
        public static int DigitSum(this int a, int b)
        {
            return a + b;
        }


        /// <summary>
        /// Calculates the subtraction of a number.
        /// </summary>
        /// <param name="number">The base number.</param>
        /// <param name="subtrahend">The number to subtract.</param>
        /// <returns>The result of the subtraction.</returns>
        public static int Subtract(this int number, int subtrahend)
        {
            return number - subtrahend;
        }

        /// <summary>
        /// Finds the greatest common divisor (GCD) of two numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The GCD of the two numbers.</returns>
        public static int GreatestCommonDivisor(this int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
        /// <summary>
        /// Finds the greatest common multiple (LCM) of two numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The LCM of the two numbers.</returns>
        public static int GreatestCommonMultiple(this int a, int b)
        {
            return Math.Abs(a * b) / a.GreatestCommonDivisor(b);
        }

        /// <summary>
        /// Calculates the percentage of a number.
        /// </summary>
        /// <param name="number">The base number.</param>
        /// <param name="percentage">The percentage to calculate.</param>
        /// <returns>The calculated percentage.</returns>
        public static double Percentage(this int number, double percentage)
        {
            return (number * percentage) / 100.0;
        }


        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">The degrees to convert.</param>
        /// <returns>The equivalent in radians.</returns>
        public static double ToRadians(this int degrees)
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
    }
}
