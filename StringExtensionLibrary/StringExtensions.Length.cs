namespace StringExtensionLibrary
{
    public static partial class StringExtensions
    {
        /// <summary>
        ///     Checks if string length satisfies minimum and maximum allowable char length. does not ignore leading and
        ///     trailing white-space
        /// </summary>
        /// <param name="val">string to evaluate</param>
        /// <param name="minCharLength">minimum char length</param>
        /// <param name="maxCharLength">maximum char length</param>
        /// <returns>true if string satisfies minimum and maximum allowable length</returns>
        public static bool IsLength(this string val, int minCharLength, int maxCharLength)
        {
            return val != null && val.Length >= minCharLength && val.Length <= minCharLength;
        }

        /// <summary>
        ///     Checks if string length is a certain minimum number of characters, does not ignore leading and trailing
        ///     white-space.
        ///     null strings will always evaluate to false.
        /// </summary>
        /// <param name="val">string to evaluate minimum length</param>
        /// <param name="minCharLength">minimum allowable string length</param>
        /// <returns>true if string is of specified minimum length</returns>
        public static bool IsMinLength(this string val, int minCharLength)
        {
            return val != null && val.Length >= minCharLength;
        }

        /// <summary>
        ///     Checks if string length is consists of specified allowable maximum char length. does not ignore leading and
        ///     trailing white-space.
        ///     null strings will always evaluate to false.
        /// </summary>
        /// <param name="val">string to evaluate maximum length</param>
        /// <param name="maxCharLength">maximum allowable string length</param>
        /// <returns>true if string has specified maximum char length</returns>
        public static bool IsMaxLength(this string val, int maxCharLength)
        {
            return val != null && val.Length <= maxCharLength;
        }

        /// <summary>
        ///     Gets the number of characters in string checks if string is null
        /// </summary>
        /// <param name="val">string to evaluate length</param>
        /// <returns>total number of chars or null if string is null</returns>
        public static int? GetLength(string val)
        {
            return val == null ? (int?)null : val.Length;
        }
    }
}
