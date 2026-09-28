using System;
using System.Globalization;
using System.Text;

namespace StringExtensionLibrary
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// Converts a string to snake_case format.
        /// Handles camelCase, PascalCase, hyphenated and space-delimited text.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The snake_case representation, or string.Empty if null or whitespace.</returns>
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            return ConvertCaseWithSeparator(input.Trim(), '_');
        }

        /// <summary>
        /// Converts a string to kebab-case format.
        /// Handles camelCase, PascalCase, underscored and space-delimited text.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The kebab-case representation, or string.Empty if null or whitespace.</returns>
        public static string ToKebabCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            return ConvertCaseWithSeparator(input.Trim(), '-');
        }

        /// <summary>
        /// Converts a string to an ASCII URL-friendly slug.
        /// Strips diacritics, normalizes whitespace and replaces special characters with hyphens.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The sanitized URL slug, or string.Empty if null or whitespace.</returns>
        public static string ToSlug(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            string normalized = RemoveDiacritics(input.Trim());
            var builder = new StringBuilder(normalized.Length);
            bool lastWasHyphen = false;

            for (int i = 0; i < normalized.Length; i++)
            {
                char c = char.ToLowerInvariant(normalized[i]);

                if (IsSlugAllowedCharacter(c))
                {
                    builder.Append(c);
                    lastWasHyphen = false;
                }
                else if (char.IsWhiteSpace(c) || c == '-' || c == '_' || c == '.' || c == '/')
                {
                    if (!lastWasHyphen && builder.Length > 0)
                    {
                        builder.Append('-');
                        lastWasHyphen = true;
                    }
                }
            }

            return TrimTrailingHyphens(builder);
        }

        private static string ConvertCaseWithSeparator(string text, char separator)
        {
            var builder = new StringBuilder(text.Length + Math.Max(4, text.Length / 4));
            bool previousWasSeparator = false;

            for (int i = 0; i < text.Length; i++)
            {
                char current = text[i];

                if (current == '_' || current == '-' || char.IsWhiteSpace(current))
                {
                    if (!previousWasSeparator && builder.Length > 0)
                    {
                        builder.Append(separator);
                        previousWasSeparator = true;
                    }
                    continue;
                }

                if (char.IsUpper(current))
                {
                    if (ShouldPrependSeparator(text, i, previousWasSeparator, builder.Length))
                    {
                        builder.Append(separator);
                    }

                    builder.Append(char.ToLowerInvariant(current));
                    previousWasSeparator = false;
                    continue;
                }

                builder.Append(current);
                previousWasSeparator = false;
            }

            return builder.ToString();
        }

        private static bool ShouldPrependSeparator(string text, int currentIndex, bool previousWasSeparator, int currentOutputLength)
        {
            if (currentOutputLength == 0 || previousWasSeparator)
            {
                return false;
            }

            char previousChar = text[currentIndex - 1];

            if (char.IsLower(previousChar) || char.IsDigit(previousChar))
            {
                return true;
            }

            bool hasNextChar = currentIndex + 1 < text.Length;
            if (char.IsUpper(previousChar) && hasNextChar && char.IsLower(text[currentIndex + 1]))
            {
                return true;
            }

            return false;
        }

        private static string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private static bool IsSlugAllowedCharacter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9');
        }

        private static string TrimTrailingHyphens(StringBuilder builder)
        {
            while (builder.Length > 0 && builder[builder.Length - 1] == '-')
            {
                builder.Length--;
            }

            return builder.ToString();
        }
    }
}
