using System;
using System.Text;

namespace EasyToolkit.Core.Textual
{
    /// <summary>
    /// Provides extension methods for converting strings between different naming conventions.
    /// </summary>
    /// <remarks>
    /// Supported naming conventions:
    /// <list type="bullet">
    /// <item><description>PascalCase: FirstWordCapitalized</description></item>
    /// <item><description>camelCase: firstWordLowercase</description></item>
    /// <item><description>snake_case: words_separated_by_underscores</description></item>
    /// <item><description>SCREAMING_SNAKE_CASE: WORDS_SEPARATED_BY_UNDERSCORES</description></item>
    /// <item><description>kebab-case: words-separated-by-hyphens</description></item>
    /// <item><description>SCREAMING-KEBAB-CASE: WORDS-SEPARATED-BY-HYPHENS</description></item>
    /// </list>
    /// </remarks>
    public static class StringNamingExtensions
    {
        /// <summary>
        /// Converts a string from any supported naming convention to PascalCase.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The string converted to PascalCase, or empty string if input is null or empty.</returns>
        public static string ToPascalCase(this string input)
        {
            if (input.IsNullOrEmpty())
                return string.Empty;

            string[] words = SplitIntoWords(input);
            var builder = new StringBuilder();

            foreach (string word in words)
            {
                if (word.Length > 0)
                {
                    builder.Append(char.ToUpperInvariant(word[0]));
                    if (word.Length > 1)
                    {
                        builder.Append(word[1..].ToLowerInvariant());
                    }
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Converts a string from any supported naming convention to camelCase.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The string converted to camelCase, or empty string if input is null or empty.</returns>
        public static string ToCamelCase(this string input)
        {
            if (input.IsNullOrEmpty())
                return string.Empty;

            string pascalCase = input.ToPascalCase();
            if (pascalCase.Length == 0)
                return string.Empty;

            return char.ToLowerInvariant(pascalCase[0]) + pascalCase[1..];
        }

        /// <summary>
        /// Converts a string from any supported naming convention to snake_case.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The string converted to snake_case, or empty string if input is null or empty.</returns>
        public static string ToSnakeCase(this string input)
        {
            if (input.IsNullOrEmpty())
                return string.Empty;

            string[] words = SplitIntoWords(input);
            return string.Join("_", words).ToLowerInvariant();
        }

        /// <summary>
        /// Converts a string from any supported naming convention to SCREAMING_SNAKE_CASE.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The string converted to SCREAMING_SNAKE_CASE, or empty string if input is null or empty.</returns>
        public static string ToScreamingSnakeCase(this string input)
        {
            if (input.IsNullOrEmpty())
                return string.Empty;

            string[] words = SplitIntoWords(input);
            return string.Join("_", words).ToUpperInvariant();
        }

        /// <summary>
        /// Converts a string from any supported naming convention to kebab-case.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The string converted to kebab-case, or empty string if input is null or empty.</returns>
        public static string ToKebabCase(this string input)
        {
            if (input.IsNullOrEmpty())
                return string.Empty;

            string[] words = SplitIntoWords(input);
            return string.Join("-", words).ToLowerInvariant();
        }

        /// <summary>
        /// Converts a string from any supported naming convention to SCREAMING-KEBAB-CASE.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The string converted to SCREAMING-KEBAB-CASE, or empty string if input is null or empty.</returns>
        public static string ToScreamingKebabCase(this string input)
        {
            if (input.IsNullOrEmpty())
                return string.Empty;

            string[] words = SplitIntoWords(input);
            return string.Join("-", words).ToUpperInvariant();
        }

        /// <summary>
        /// Splits a string into individual words based on naming convention patterns.
        /// </summary>
        /// <param name="input">The string to split.</param>
        /// <returns>An array of words extracted from the input string.</returns>
        /// <remarks>
        /// This method detects word boundaries based on:
        /// <list type="bullet">
        /// <item><description>Uppercase letters (for camelCase and PascalCase)</description></item>
        /// <item><description>Underscores (for snake_case)</description></item>
        /// <item><description>Hyphens (for kebab-case)</description></item>
        /// </list>
        /// </remarks>
        private static string[] SplitIntoWords(string input)
        {
            if (input.IsNullOrEmpty())
                return Array.Empty<string>();

            var words = new System.Collections.Generic.List<string>();
            var currentWord = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                // Skip delimiters but mark word boundary
                if (c == '_' || c == '-')
                {
                    if (currentWord.Length > 0)
                    {
                        words.Add(currentWord.ToString());
                        currentWord.Clear();
                    }
                    continue;
                }

                // Handle uppercase letters (word boundary for camelCase/PascalCase)
                if (char.IsUpper(c))
                {
                    // Check if this is the start of a new word
                    // A new word starts when:
                    // 1. Previous char was lowercase (e.g., "myName" -> "my", "Name")
                    // 2. Next char is lowercase and previous was uppercase (e.g., "XMLParser" -> "XML", "Parser")
                    bool isNewWord = false;

                    if (currentWord.Length > 0)
                    {
                        char prevChar = currentWord[currentWord.Length - 1];

                        if (char.IsLower(prevChar))
                        {
                            // Case: "myName" - 'm' 'y' are lowercase, 'N' starts new word
                            isNewWord = true;
                        }
                        else if (char.IsDigit(prevChar))
                        {
                            // Case: "variable123Name" - '3' is digit, 'N' starts new word
                            isNewWord = true;
                        }
                        else if (char.IsUpper(prevChar) && i + 1 < input.Length && char.IsLower(input[i + 1]))
                        {
                            // Case: "XMLParser" - 'L' is uppercase, 'P' is uppercase, 'a' is lowercase
                            // So 'P' starts new word
                            isNewWord = true;
                        }
                    }

                    if (isNewWord)
                    {
                        words.Add(currentWord.ToString());
                        currentWord.Clear();
                    }
                }

                currentWord.Append(c);
            }

            // Add the last word
            if (currentWord.Length > 0)
            {
                words.Add(currentWord.ToString());
            }

            return words.ToArray();
        }
    }
}
