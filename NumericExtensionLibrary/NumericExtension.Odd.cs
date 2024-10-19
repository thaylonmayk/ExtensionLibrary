namespace NumericExtensionLibrary
{
    public static partial class NumericExtension
    {

        /// <summary>
        /// Checks if a number is odd.
        /// </summary>
        /// <param name="number">The number to check.</param>
        /// <returns>True if the number is odd; otherwise, false.</returns>
        public static bool IsOdd(this int number)
        {
            return number % 2 != 0;

        }
    }
}
