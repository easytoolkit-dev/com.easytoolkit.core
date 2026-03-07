using System;
using System.Collections.Generic;

namespace EasyToolkit.Core.Textual
{
    /// <summary>
    /// Provides extension methods for string validation.
    /// </summary>
    public static class StringValidationExtensions
    {
        private static readonly HashSet<string> CSharpKeywords = new HashSet<string>(StringComparer.Ordinal)
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
            "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else",
            "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for",
            "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is",
            "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override",
            "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed",
            "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw",
            "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using",
            "virtual", "void", "volatile", "while"
        };

        /// <summary>
        /// Validates if the string is a valid C# identifier. Throws an exception if invalid.
        /// </summary>
        /// <param name="identifier">The string to validate.</param>
        /// <exception cref="InvalidIdentifierException">Thrown when the identifier is invalid.</exception>
        public static void ValidateIdentifier(this string identifier)
        {
            if (!IsValidIdentifier(identifier, out var error))
            {
                throw new InvalidIdentifierException(error.Value, identifier);
            }
        }

        /// <summary>
        /// Checks if the string is a valid C# identifier and returns the error type if invalid.
        /// </summary>
        /// <param name="identifier">The string to check.</param>
        /// <param name="error">Output parameter containing the error type if invalid, or null if valid.</param>
        /// <returns>True if the string is a valid identifier; otherwise, false.</returns>
        public static bool IsValidIdentifier(this string identifier, out IdentifierValidationError? error)
        {
            error = null;

            if (identifier.IsNullOrWhiteSpace())
            {
                error = IdentifierValidationError.Empty;
                return false;
            }

            if (!char.IsLetter(identifier[0]) && identifier[0] != '_')
            {
                error = IdentifierValidationError.InvalidFirstCharacter;
                return false;
            }

            for (int i = 1; i < identifier.Length; i++)
            {
                if (!char.IsLetterOrDigit(identifier[i]) && identifier[i] != '_')
                {
                    error = IdentifierValidationError.InvalidCharacter;
                    return false;
                }
            }

            if (CSharpKeywords.Contains(identifier))
            {
                error = IdentifierValidationError.ReservedKeyword;
                return false;
            }

            return true;
        }
    }
}
