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
            if (number < 0) throw new ArgumentException("Number must be non-negative.");
            return number == 0 ? 1 : number * Factorial(number - 1);
        }
    }
}
