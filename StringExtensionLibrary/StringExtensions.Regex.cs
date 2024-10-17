using System.Text.RegularExpressions;

namespace StringExtensionLibrary
{
    public static partial class StringExtensions
    {
        /// <summary>
        ///     Replace Line Feeds
        /// </summary>
        /// <param name="val">string to remove line feeds</param>
        /// <returns>System.string</returns>
        public static string ReplaceLineFeeds(this string val)
        {
            return Regex.Replace(val, @"^[\r\n]+|\.|[\r\n]+$", "");
        }

        /// <summary>
        ///     Validates if a string is valid IPv4
        ///     Regular expression taken from <a href="http://regexlib.com/REDetails.aspx?regexp_id=2035">Regex reference</a>
        /// </summary>
        /// <param name="val">string IP address</param>
        /// <returns>true if string matches valid IP address else false</returns>
        public static bool IsValidIPv4(this string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            return Regex.Match(val,
                @"(?:^|\s)([a-z]{3,6}(?=://))?(://)?((?:25[0-5]|2[0-4]\d|[01]?\d\d?)\.(?:25[0-5]|2[0-4]\d|[01]?\d\d?)\.(?:25[0-5]|2[0-4]\d|[01]?\d\d?)\.(?:25[0-5]|2[0-4]\d|[01]?\d\d?))(?::(\d{2,5}))?(?:\s|$)")
                .Success;
        }

        /// <summary>
        ///     Validate email address
        /// </summary>
        /// <param name="email">string email address</param>
        /// <returns>true or false if email if valid</returns>
        public static bool IsEmailAddress(this string email)
        {
            string pattern =
                "^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$";
            return Regex.Match(email, pattern).Success;
        }

        /// <summary>
        ///     Count number of occurrences in string
        /// </summary>
        /// <param name="val">string containing text</param>
        /// <param name="stringToMatch">string or pattern find</param>
        /// <returns></returns>
        public static int CountOccurrences(this string val, string stringToMatch)
        {
            return Regex.Matches(val, stringToMatch, RegexOptions.IgnoreCase).Count;
        }


    }
}
