using System.Collections.Generic;

namespace NumericExtensionLibrary
{
    public static partial class NumericExtension
    {

        /// <summary>
        /// Calculates the average of a list of numbers.
        /// </summary>
        /// <param name="numbers">The list of numbers.</param>
        /// <returns>The average of the numbers.</returns>
        public static double Average(this List<int> numbers)
        {
            return numbers.Average();
        }
    }
}
