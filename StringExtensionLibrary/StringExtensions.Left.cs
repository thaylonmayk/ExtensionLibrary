using System;

namespace StringExtensionLibrary
{
    public static partial class StringExtensions
    {
        /// <summary>
        ///     Extracts the left part of the input string limited with the length parameter
        /// </summary>
        /// <param name="val">The input string to take the left part from</param>
        /// <param name="length">The total number characters to take from the input string</param>
        /// <returns>The substring starting at startIndex 0 until length</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Length is smaller than zero or higher than the length of input</exception>
        public static string Left(this string val, int length)
        {
            if (string.IsNullOrEmpty(val))
            {
                throw new ArgumentNullException("val");
            }
            if (length < 0 || length > val.Length)
            {
                throw new ArgumentOutOfRangeException("length",
                    "length cannot be higher than total string length or less than 0");
            }
            return val.Substring(0, length);
        }

        /// <summary>
        /// Extracts the left part of the input string limited by the first character
        /// </summary>
        /// <param name="input">The input string to take the left part from</param>
        /// <param name="character">The character to find in the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the character (excluding the character) or the whole input string if the character was not found</returns>
        public static string LeftOf(this string input, char character)
        {
            return LeftOf(input, character, 0);
        }

        /// <summary>
        /// Extracts the left part of the input string limited by the first character
        /// </summary>
        /// <param name="input">The input string to take the left part from</param>
        /// <param name="character">The character to find in the input string</param>
        /// <param name="skip">The numbers of found characters to skip before taking the left part</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the character (excluding the character) or the whole input string if the character was not found</returns>
        public static string LeftOf(this string input, char character, int skip)
        {
            // preconditions
            if (input == null)
                throw new ArgumentNullException("input");
            if (skip < 0)
                throw new ArgumentOutOfRangeException("skip", "skip should be larger or equal to 0");

            string result;

            if (input.Length == 0)
            {
                result = input;
            }
            else
            {
                int characterPosition = 0;
                int charactersFound = -1;

                while (charactersFound < skip)
                {
                    characterPosition = input.IndexOf(character, characterPosition + 1);
                    if (characterPosition == -1)
                    {
                        break;
                    }
                    else
                    {
                        charactersFound++;
                    }
                }

                if (characterPosition == -1)
                {
                    result = input;
                }
                else
                {
                    result = input.Substring(0, characterPosition);
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts the left part of the input string limited by the first occurrence of value
        /// </summary>
        /// <param name="input">The input string to take the left part from</param>
        /// <param name="value">The value to find in the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the first occurrence of value or the whole input string if the value was not found</returns>
        public static string LeftOf(this string input, string value)
        {
            return LeftOf(input, value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Extracts the left part of the input string limited by the first occurrence of value
        /// </summary>
        /// <param name="input">The input string to take the left part from</param>
        /// <param name="value">The value to find in the input string</param>
        /// <param name="comparisonType">The way startsWith should be compared to the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the first occurrence of value or the whole input string if the value was not found</returns>
        public static string LeftOf(this string input, string value, StringComparison comparisonType)
        {
            return LeftOf(input, value, 0, comparisonType);
        }

        /// <summary>
        /// Extracts the left part of the input string limited by the n'th occurrence of value
        /// </summary>
        /// <param name="input">The input string to take the left part from</param>
        /// <param name="value">The value to find in the input string</param>
        /// <param name="skip">The numbers of found values to skip before taking the left part</param>
        /// <param name="comparisonType">The way startsWith should be compared to the input string</param>
        /// <returns>The substring starting at startIndex 0 until either the position of the n'th occurrence of value or the whole input string if the value was not found</returns>
        public static string LeftOf(this string input, string value, int skip, StringComparison comparisonType)
        {
            // preconditions
            if (input == null)
                throw new ArgumentNullException("input");
            if (value == null)
                throw new ArgumentNullException("value");
            if (skip < 0)
                throw new ArgumentOutOfRangeException("skip", "skip should be larger or equal to 0");

            string result;

            if (input.Length <= skip)
            {
                result = input;
            }
            else
            {
                int valuePosition = 0;
                int valuesFound = -1;

                while (valuesFound < skip)
                {
                    valuePosition = input.IndexOf(value, valuePosition + 1, comparisonType);
                    if (valuePosition == -1)
                    {
                        break;
                    }
                    else
                    {
                        valuesFound++;
                    }
                }

                if (valuePosition == -1)
                {
                    result = input;
                }
                else
                {
                    result = input.Substring(0, valuePosition);
                }
            }

            return result;
        }
    }
}
