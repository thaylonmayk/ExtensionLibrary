using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericExtensionLibrary
{
    public static partial class NumericExtension
    {
        /// <summary>
        /// Converts a number to a binary string.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>The binary representation of the number.</returns>
        public static string ToBinaryString(this int number)
        {
            return Convert.ToString(number, 2);
        }

        /// <summary>
        /// Converts a number to a hexadecimal string.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>The hexadecimal representation of the number.</returns>
        public static string ToHexString(this int number)
        {
            return Convert.ToString(number, 16).ToUpper();
        }

    }
}
