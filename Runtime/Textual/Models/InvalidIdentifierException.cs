using System;

namespace EasyToolkit.Core.Textual
{
    /// <summary>
    /// Exception thrown when an identifier string is invalid.
    /// </summary>
    public class InvalidIdentifierException : ArgumentException
    {
        /// <summary>
        /// Gets the type of validation error.
        /// </summary>
        public IdentifierValidationError ErrorType { get; }

        /// <summary>
        /// Gets the invalid identifier string.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidIdentifierException"/> class.
        /// </summary>
        /// <param name="errorType">The type of validation error.</param>
        /// <param name="identifier">The invalid identifier.</param>
        public InvalidIdentifierException(IdentifierValidationError errorType, string identifier)
            : base(GetMessage(errorType, identifier))
        {
            ErrorType = errorType;
            Identifier = identifier;
        }

        private static string GetMessage(IdentifierValidationError errorType, string identifier)
        {
            switch (errorType)
            {
                case IdentifierValidationError.Empty:
                    return
                        "Identifier cannot be empty or null. ensure the identifier string contains valid characters.";
                case IdentifierValidationError.InvalidFirstCharacter:
                    return
                        $"Identifier '{identifier}' starts with an invalid character. Identifiers must begin with a letter or an underscore.";
                case IdentifierValidationError.InvalidCharacter:
                    return
                        $"Identifier '{identifier}' contains invalid characters. Identifiers can only contain letters, numbers, or underscores.";
                case IdentifierValidationError.ReservedKeyword:
                    return
                        $"Identifier '{identifier}' is a reserved C# keyword. Choose a different name that does not conflict with C# keywords.";
                default:
                    return $"Identifier '{identifier}' is invalid. Error code: {errorType}.";
            }
        }
    }
}
