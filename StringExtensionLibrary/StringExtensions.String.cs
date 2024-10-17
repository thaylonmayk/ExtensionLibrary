using System;
using System.Linq;
using System.Text;

namespace StringExtensionLibrary
{
    public static partial class StringExtensions
    {
        /// <summary>
        ///     Read in a sequence of words from standard input and capitalize each
        ///     one (make first letter uppercase; make rest lowercase).
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>Word with capitalization</returns>
        public static string Capitalize(this string s)
        {
            if (s.Length == 0)
            {
                return s;
            }
            return s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower();
        }

        /// <summary>
        ///     Gets first character in string
        /// </summary>
        /// <param name="val">val</param>
        /// <returns>System.string</returns>
        public static string FirstCharacter(this string val)
        {
            return (!string.IsNullOrEmpty(val))
                ? (val.Length >= 1)
                    ? val.Substring(0, 1)
                    : val
                : null;
        }

        /// <summary>
        ///     Gets last character in string
        /// </summary>
        /// <param name="val">val</param>
        /// <returns>System.string</returns>
        public static string LastCharacter(this string val)
        {
            return (!string.IsNullOrEmpty(val))
                ? (val.Length >= 1)
                    ? val.Substring(val.Length - 1, 1)
                    : val
                : null;
        }

        /// <summary>
        ///     Check a String ends with another string ignoring the case.
        /// </summary>
        /// <param name="val">string</param>
        /// <param name="suffix">suffix</param>
        /// <returns>true or false</returns>
        public static bool EndsWithIgnoreCase(this string val, string suffix)
        {
            if (val == null)
            {
                throw new ArgumentNullException("val", "val parameter is null");
            }
            if (suffix == null)
            {
                throw new ArgumentNullException("suffix", "suffix parameter is null");
            }
            if (val.Length < suffix.Length)
            {
                return false;
            }
            return val.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        ///     Check a String starts with another string ignoring the case.
        /// </summary>
        /// <param name="val">string</param>
        /// <param name="prefix">prefix</param>
        /// <returns>true or false</returns>
        public static bool StartsWithIgnoreCase(this string val, string prefix)
        {
            if (val == null)
            {
                throw new ArgumentNullException("val", "val parameter is null");
            }
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix", "prefix parameter is null");
            }
            if (val.Length < prefix.Length)
            {
                return false;
            }
            return val.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        ///     Removes the first part of the string, if no match found return original string
        /// </summary>
        /// <param name="val">string to remove prefix</param>
        /// <param name="prefix">prefix</param>
        /// <param name="ignoreCase">Indicates whether the compare should ignore case</param>
        /// <returns>trimmed string with no prefix or original string</returns>
        public static string RemovePrefix(this string val, string prefix, bool ignoreCase = true)
        {
            if (!string.IsNullOrEmpty(val) && (ignoreCase ? val.StartsWithIgnoreCase(prefix) : val.StartsWith(prefix)))
            {
                return val.Substring(prefix.Length, val.Length - prefix.Length);
            }
            return val;
        }

        /// <summary>
        ///     Removes the end part of the string, if no match found return original string
        /// </summary>
        /// <param name="val">string to remove suffix</param>
        /// <param name="suffix">suffix</param>
        /// <param name="ignoreCase">Indicates whether the compare should ignore case</param>
        /// <returns>trimmed string with no suffix or original string</returns>
        public static string RemoveSuffix(this string val, string suffix, bool ignoreCase = true)
        {
            if (!string.IsNullOrEmpty(val) && (ignoreCase ? val.EndsWithIgnoreCase(suffix) : val.EndsWith(suffix)))
            {
                return val.Substring(0, val.Length - suffix.Length);
            }
            return null;
        }

        /// <summary>
        ///     Remove Characters from string
        /// </summary>
        /// <param name="s">string to remove characters</param>
        /// <param name="chars">array of chars</param>
        /// <returns>System.string</returns>
        public static string RemoveChars(this string s, params char[] chars)
        {
            var sb = new StringBuilder(s.Length);
            foreach (char c in s.Where(c => !chars.Contains(c)))
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Reverse string
        /// </summary>
        /// <param name="val">string to reverse</param>
        /// <returns>System.string</returns>
        public static string Reverse(this string val)
        {
            var chars = new char[val.Length];
            for (int i = val.Length - 1, j = 0; i >= 0; --i, ++j)
            {
                chars[j] = val[i];
            }
            val = new String(chars);
            return val;
        }


        /// <summary>
        ///     Appends the suffix to the end of the string if the string does not already end in the suffix.
        /// </summary>
        /// <param name="val">string to append suffix</param>
        /// <param name="suffix">suffix</param>
        /// <param name="ignoreCase">Indicates whether the compare should ignore case</param>
        /// <returns></returns>
        public static string AppendSuffixIfMissing(this string val, string suffix, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(val) || (ignoreCase ? val.EndsWithIgnoreCase(suffix) : val.EndsWith(suffix)))
            {
                return val;
            }
            return val + suffix;
        }

        /// <summary>
        ///     Appends the prefix to the start of the string if the string does not already start with prefix.
        /// </summary>
        /// <param name="val">string to append prefix</param>
        /// <param name="prefix">prefix</param>
        /// <param name="ignoreCase">Indicates whether the compare should ignore case</param>
        /// <returns></returns>
        public static string AppendPrefixIfMissing(this string val, string prefix, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(val) || (ignoreCase ? val.StartsWithIgnoreCase(prefix) : val.StartsWith(prefix)))
            {
                return val;
            }
            return prefix + val;
        }

        /// <summary>
        ///     Check if a string does not start with prefix
        /// </summary>
        /// <param name="val">string to evaluate</param>
        /// <param name="prefix">prefix</param>
        /// <returns>true if string does not match prefix else false, null values will always evaluate to false</returns>
        public static bool DoesNotStartWith(this string val, string prefix)
        {
            return val == null || prefix == null ||
                   !val.StartsWith(prefix, StringComparison.InvariantCulture);
        }

        /// <summary>
        ///     Check if a string does not end with prefix
        /// </summary>
        /// <param name="val">string to evaluate</param>
        /// <param name="suffix">suffix</param>
        /// <returns>true if string does not match prefix else false, null values will always evaluate to false</returns>
        public static bool DoesNotEndWith(this string val, string suffix)
        {
            return val == null || suffix == null ||
                   !val.EndsWith(suffix, StringComparison.InvariantCulture);
        }
    }
}
