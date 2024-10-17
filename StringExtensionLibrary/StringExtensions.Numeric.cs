using System;
using System.Globalization;

namespace StringExtensionLibrary
{
    public static partial class StringExtensions
    {

        /// <summary>
        ///     IsNumeric checks if a string is a valid floating value
        /// </summary>
        /// <param name="val"></param>
        /// <returns>Boolean True if isNumeric else False</returns>
        /// <remarks></remarks>
        public static bool IsNumeric(this string val)
        {
            // Variable to collect the Return value of the TryParse method.

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            double retNum;

            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            bool isNum = Double.TryParse(val, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        /// <summary>
        ///     IsInteger Function checks if a string is a valid int32 value
        /// </summary>
        /// <param name="val">val</param>
        /// <returns>Boolean True if isInteger else False</returns>
        public static bool IsInteger(this string val)
        {
            // Variable to collect the Return value of the TryParse method.

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            int retNum;

            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            bool isNum = Int32.TryParse(val, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }


        /// <summary>
        ///     Converts the string representation of a number to its 32-bit signed integer equivalent
        /// </summary>
        /// <param name="value">string containing a number to convert</param>
        /// <returns>System.Int32</returns>
        /// <remarks>
        ///     The conversion fails if the string parameter is null, is not of the correct format, or represents a number
        ///     less than System.Int32.MinValue or greater than System.Int32.MaxValue
        /// </remarks>
        public static int ToInt32(this string value)
        {
            int number;
            Int32.TryParse(value, out number);
            return number;
        }

        /// <summary>
        ///     Converts the string representation of a number to its 64-bit signed integer equivalent
        /// </summary>
        /// <param name="value">string containing a number to convert</param>
        /// <returns>System.Int64</returns>
        /// <remarks>
        ///     The conversion fails if the string parameter is null, is not of the correct format, or represents a number
        ///     less than System.Int64.MinValue or greater than System.Int64.MaxValue
        /// </remarks>
        public static long ToInt64(this string value)
        {
            long number;
            Int64.TryParse(value, out number);
            return number;
        }

        /// <summary>
        ///     Converts the string representation of a number to its 16-bit signed integer equivalent
        /// </summary>
        /// <param name="value">string containing a number to convert</param>
        /// <returns>System.Int16</returns>
        /// <remarks>
        ///     The conversion fails if the string parameter is null, is not of the correct format, or represents a number
        ///     less than System.Int16.MinValue or greater than System.Int16.MaxValue
        /// </remarks>
        public static short ToInt16(this string value)
        {
            short number;
            Int16.TryParse(value, out number);
            return number;
        }

    }
}
