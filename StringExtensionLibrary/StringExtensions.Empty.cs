using System;

namespace StringExtensionLibrary
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// Determines whether the input string is empty (0 characters)
        /// </summary>
        /// <param name="input">The input string to check</param>
        /// <returns>True if the input string is empty</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        public static bool IsEmpty(this string input)
        {
            // preconditions
            if (input == null)
                throw new ArgumentNullException("input");

            return input.Length == 0;
        }

        /// <summary>
        ///     Checks if a string is null or empty
        /// </summary>
        /// <param name="val">string to evaluate</param>
        /// <returns>true if string is null or is empty else false</returns>
        public static bool IsNullOrEmpty(this string val)
        {
            return String.IsNullOrEmpty(val);
        }
        /// <summary>
        ///     Checks if a string is null or consists of only whitespaces
        /// </summary>
        /// <param name="val">string to evaluate</param>
        /// <returns>true if string is null or consists of only whitespaces else false</returns>
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return String.IsNullOrWhiteSpace(input);

        }
        /// <summary>
        ///     Gets empty String if passed value is of type Null/Nothing
        /// </summary>
        /// <param name="val">val</param>
        /// <returns>System.String</returns>
        /// <remarks></remarks>
        public static string GetEmptyStringIfNull(this string val)
        {
            return (val != null ? val.Trim() : "");
        }
    }
}
