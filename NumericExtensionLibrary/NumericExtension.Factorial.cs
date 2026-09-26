using System;

namespace NumericExtensionLibrary
{
    public static partial class NumericExtension
    {

        /// <summary>
        /// Finds the factorial of a number.
        /// </summary>
        /// <param name="number">The number to find the factorial of.</param>
        /// <returns>The factorial of the number.</returns>
        public static long Factorial(this int number)
        {
            if (number < 0) throw new ArgumentOutOfRangeException(nameof(number), "Number must be non-negative.");
            if (number > 20) throw new ArgumentOutOfRangeException(nameof(number), "Factorial input cannot exceed 20 to prevent 64-bit integer overflow.");

            long result = 1;
            for (int factor = 2; factor <= number; factor++)
            {
                result *= factor;
            }

            return result;
        }
    }
}
