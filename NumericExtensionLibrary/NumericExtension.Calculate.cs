using System;

namespace NumericExtensionLibrary
{
    public static partial class NumericExtension
    {


        /// <summary>
        /// Calculates the sum of the digits of a number.
        /// </summary>
        /// <param name="number">The number to calculate the sum of digits for.</param>
        /// <returns>The sum of the digits.</returns>
        public static int DigitSum(this int number)
        {
            long n = Math.Abs((long)number);
            int sum = 0;
            while (n > 0)
            {
                sum += (int)(n % 10);
                n /= 10;
            }
            return sum;
        }

        /// <summary>
        /// Calculates the sum of the digits of a 64-bit number.
        /// </summary>
        /// <param name="number">The number to calculate the sum of digits for.</param>
        /// <returns>The sum of the digits.</returns>
        public static int DigitSum(this long number)
        {
            long n = number == long.MinValue ? long.MaxValue : Math.Abs(number);
            int sum = 0;
            while (n > 0)
            {
                sum += (int)(n % 10);
                n /= 10;
            }
            return sum;
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
            long x = Math.Abs((long)a);
            long y = Math.Abs((long)b);

            while (y != 0)
            {
                long temp = y;
                y = x % y;
                x = temp;
            }
            return (int)x;
        }

        /// <summary>
        /// Finds the greatest common multiple (LCM) of two numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The LCM of the two numbers, or 0 if either number is 0.</returns>
        public static int GreatestCommonMultiple(this int a, int b)
        {
            if (a == 0 || b == 0) return 0;
            int gcd = a.GreatestCommonDivisor(b);
            if (gcd == 0) return 0;

            long product = Math.Abs((long)a * (long)b);
            return (int)(product / gcd);
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
