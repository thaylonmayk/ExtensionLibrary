namespace StringExtensionLibrary
{
    public static partial class StringExtensions
    {
        /// <summary>
        ///     Appends String quotes for type CSV data
        /// </summary>
        /// <param name="val">val</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ParseStringToCsv(this string val)
        {
            return '"' + GetEmptyStringIfNull(val).Replace("\"", "\"\"") + '"';
        }
    }
}
