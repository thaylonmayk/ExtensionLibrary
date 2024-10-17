using System;

namespace NumericExtensionLibrary
{
    public static partial class NumericExtension
    {

        /// <summary>
        /// Checks if a number is prime.
        /// </summary>
        /// <param name="number">The number to check.</param>
        /// <returns>True if the number is prime; otherwise, false.</returns>
        public static bool IsPrime(this int number)
        {
            if (number <= 1) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;

        }

        /// <summary>
        /// Checks if a number is even.
        /// </summary>
        /// <param name="number">The number to check.</param>
        /// <returns>True if the number is even; otherwise, false.</returns>
        public static bool IsEven(this int number)
        {
            return number % 2 == 0;
        }

        /// <summary>
        /// Checks if a number is a perfect square.
        /// </summary>
        /// <param name="number">The number to check.</param>
        /// <returns>True if the number is a perfect square; otherwise, false.</returns>
        public static bool IsPerfectSquare(this int number)
        {
            int sqrt = (int)Math.Sqrt(number);
            return sqrt * sqrt == number;
        }
        /// <summary>
        /// Checks if a number is a multiple of another number.
        /// </summary>
        /// <param name="number">The number to check.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>True if the number is a multiple of the divisor; otherwise, false.</returns>
        public static bool IsMultipleOf(this int number, int divisor)
        {
            return number % divisor == 0;
        }
    }
}
