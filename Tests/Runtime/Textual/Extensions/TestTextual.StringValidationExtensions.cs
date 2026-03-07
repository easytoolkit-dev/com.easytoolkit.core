using NUnit.Framework;
using System;

namespace EasyToolkit.Core.Textual.Tests
{
    /// <summary>
    /// Unit tests for StringValidationExtensions.
    /// </summary>
    public class TestStringValidationExtensions
    {
        #region IsValidIdentifier - Valid Identifiers

        /// <summary>
        /// Verifies that IsValidIdentifier returns true for a simple valid identifier.
        /// </summary>
        [Test]
        public void IsValidIdentifier_SimpleValidIdentifier_ReturnsTrue()
        {
            // Arrange
            string identifier = "MyVariable";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns true for identifier starting with underscore.
        /// </summary>
        [Test]
        public void IsValidIdentifier_StartingWithUnderscore_ReturnsTrue()
        {
            // Arrange
            string identifier = "_myVariable";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns true for identifier containing numbers.
        /// </summary>
        [Test]
        public void IsValidIdentifier_ContainingNumbers_ReturnsTrue()
        {
            // Arrange
            string identifier = "player123";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns true for single letter identifier.
        /// </summary>
        [Test]
        public void IsValidIdentifier_SingleLetter_ReturnsTrue()
        {
            // Arrange
            string identifier = "x";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns true for single underscore identifier.
        /// </summary>
        [Test]
        public void IsValidIdentifier_SingleUnderscore_ReturnsTrue()
        {
            // Arrange
            string identifier = "_";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns true for identifier with mixed letters and underscores.
        /// </summary>
        [Test]
        public void IsValidIdentifier_MixedLettersAndUnderscores_ReturnsTrue()
        {
            // Arrange
            string identifier = "my_variable_name";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns true for identifier with letters, numbers, and underscores.
        /// </summary>
        [Test]
        public void IsValidIdentifier_LettersNumbersAndUnderscores_ReturnsTrue()
        {
            // Arrange
            string identifier = "player_123_health";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(error);
        }

        #endregion

        #region IsValidIdentifier - Empty/Null

        /// <summary>
        /// Verifies that IsValidIdentifier returns false for null string.
        /// </summary>
        [Test]
        public void IsValidIdentifier_NullInput_ReturnsFalseWithEmptyError()
        {
            // Arrange
            string identifier = null;

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.Empty, error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns false for empty string.
        /// </summary>
        [Test]
        public void IsValidIdentifier_EmptyString_ReturnsFalseWithEmptyError()
        {
            // Arrange
            string identifier = string.Empty;

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.Empty, error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns false for whitespace-only string.
        /// </summary>
        [Test]
        public void IsValidIdentifier_WhitespaceOnly_ReturnsFalseWithEmptyError()
        {
            // Arrange
            string identifier = "   ";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.Empty, error);
        }

        #endregion

        #region IsValidIdentifier - Invalid First Character

        /// <summary>
        /// Verifies that IsValidIdentifier returns false when identifier starts with a digit.
        /// </summary>
        [Test]
        public void IsValidIdentifier_StartingWithDigit_ReturnsFalseWithInvalidFirstCharacterError()
        {
            // Arrange
            string identifier = "123player";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.InvalidFirstCharacter, error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns false when identifier starts with a special character.
        /// </summary>
        [Test]
        public void IsValidIdentifier_StartingWithSpecialCharacter_ReturnsFalseWithInvalidFirstCharacterError()
        {
            // Arrange
            string identifier = "@variable";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.InvalidFirstCharacter, error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns false when identifier starts with a hyphen.
        /// </summary>
        [Test]
        public void IsValidIdentifier_StartingWithHyphen_ReturnsFalseWithInvalidFirstCharacterError()
        {
            // Arrange
            string identifier = "-myVariable";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.InvalidFirstCharacter, error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns false when identifier starts with a space.
        /// </summary>
        [Test]
        public void IsValidIdentifier_StartingWithSpace_ReturnsFalseWithInvalidFirstCharacterError()
        {
            // Arrange
            string identifier = " myVariable";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.InvalidFirstCharacter, error);
        }

        #endregion

        #region IsValidIdentifier - Invalid Character

        /// <summary>
        /// Verifies that IsValidIdentifier returns false when identifier contains a hyphen.
        /// </summary>
        [Test]
        public void IsValidIdentifier_ContainingHyphen_ReturnsFalseWithInvalidCharacterError()
        {
            // Arrange
            string identifier = "my-variable";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.InvalidCharacter, error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns false when identifier contains a special character.
        /// </summary>
        [Test]
        public void IsValidIdentifier_ContainingSpecialCharacter_ReturnsFalseWithInvalidCharacterError()
        {
            // Arrange
            string identifier = "my@variable";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.InvalidCharacter, error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns false when identifier contains a space.
        /// </summary>
        [Test]
        public void IsValidIdentifier_ContainingSpace_ReturnsFalseWithInvalidCharacterError()
        {
            // Arrange
            string identifier = "my variable";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.InvalidCharacter, error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns false when identifier contains a dot.
        /// </summary>
        [Test]
        public void IsValidIdentifier_ContainingDot_ReturnsFalseWithInvalidCharacterError()
        {
            // Arrange
            string identifier = "my.variable";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.InvalidCharacter, error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns false when identifier contains invalid characters at the end.
        /// </summary>
        [Test]
        public void IsValidIdentifier_InvalidCharacterAtEnd_ReturnsFalseWithInvalidCharacterError()
        {
            // Arrange
            string identifier = "myVariable!";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.InvalidCharacter, error);
        }

        #endregion

        #region IsValidIdentifier - Reserved Keywords

        /// <summary>
        /// Verifies that IsValidIdentifier returns false for common C# keywords.
        /// </summary>
        [Test]
        public void IsValidIdentifier_CommonKeywords_ReturnsFalseWithReservedKeywordError()
        {
            // Arrange & Act & Assert
            string[] keywords = { "class", "public", "void", "int", "string", "if", "for", "return" };
            foreach (var keyword in keywords)
            {
                bool result = keyword.IsValidIdentifier(out var error);
                Assert.IsFalse(result, $"Keyword '{keyword}' should be invalid");
                Assert.AreEqual(IdentifierValidationError.ReservedKeyword, error);
            }
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns false for 'abstract' keyword.
        /// </summary>
        [Test]
        public void IsValidIdentifier_AbstractKeyword_ReturnsFalseWithReservedKeywordError()
        {
            // Arrange
            string identifier = "abstract";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.ReservedKeyword, error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns false for 'static' keyword.
        /// </summary>
        [Test]
        public void IsValidIdentifier_StaticKeyword_ReturnsFalseWithReservedKeywordError()
        {
            // Arrange
            string identifier = "static";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(IdentifierValidationError.ReservedKeyword, error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier returns true for identifier that starts with but is not exactly a keyword.
        /// </summary>
        [Test]
        public void IsValidIdentifier_KeywordPrefix_ReturnsTrue()
        {
            // Arrange
            string identifier = "myClass";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(error);
        }

        #endregion

        #region ValidateIdentifier - Valid Cases

        /// <summary>
        /// Verifies that ValidateIdentifier does not throw for valid identifiers.
        /// </summary>
        [Test]
        public void ValidateIdentifier_ValidIdentifier_DoesNotThrow()
        {
            // Arrange
            string identifier = "MyValidIdentifier123";

            // Act & Assert
            Assert.DoesNotThrow(() => identifier.ValidateIdentifier());
        }

        /// <summary>
        /// Verifies that ValidateIdentifier does not throw for identifier starting with underscore.
        /// </summary>
        [Test]
        public void ValidateIdentifier_StartingWithUnderscore_DoesNotThrow()
        {
            // Arrange
            string identifier = "_privateField";

            // Act & Assert
            Assert.DoesNotThrow(() => identifier.ValidateIdentifier());
        }

        #endregion

        #region ValidateIdentifier - Exception Cases

        /// <summary>
        /// Verifies that ValidateIdentifier throws InvalidIdentifierException for null string.
        /// </summary>
        [Test]
        public void ValidateIdentifier_NullInput_ThrowsInvalidIdentifierExceptionWithEmptyError()
        {
            // Arrange
            string identifier = null;

            // Act & Assert
            var ex = Assert.Throws<InvalidIdentifierException>(() => identifier.ValidateIdentifier());
            Assert.AreEqual(IdentifierValidationError.Empty, ex.ErrorType);
            Assert.IsNull(ex.Identifier);
        }

        /// <summary>
        /// Verifies that ValidateIdentifier throws InvalidIdentifierException for empty string.
        /// </summary>
        [Test]
        public void ValidateIdentifier_EmptyString_ThrowsInvalidIdentifierExceptionWithEmptyError()
        {
            // Arrange
            string identifier = string.Empty;

            // Act & Assert
            var ex = Assert.Throws<InvalidIdentifierException>(() => identifier.ValidateIdentifier());
            Assert.AreEqual(IdentifierValidationError.Empty, ex.ErrorType);
            Assert.AreEqual(string.Empty, ex.Identifier);
        }

        /// <summary>
        /// Verifies that ValidateIdentifier throws InvalidIdentifierException for identifier starting with digit.
        /// </summary>
        [Test]
        public void ValidateIdentifier_StartingWithDigit_ThrowsInvalidIdentifierExceptionWithInvalidFirstCharacterError()
        {
            // Arrange
            string identifier = "123Invalid";

            // Act & Assert
            var ex = Assert.Throws<InvalidIdentifierException>(() => identifier.ValidateIdentifier());
            Assert.AreEqual(IdentifierValidationError.InvalidFirstCharacter, ex.ErrorType);
            Assert.AreEqual("123Invalid", ex.Identifier);
        }

        /// <summary>
        /// Verifies that ValidateIdentifier throws InvalidIdentifierException for identifier containing special characters.
        /// </summary>
        [Test]
        public void ValidateIdentifier_ContainingSpecialCharacter_ThrowsInvalidIdentifierExceptionWithInvalidCharacterError()
        {
            // Arrange
            string identifier = "invalid@Identifier";

            // Act & Assert
            var ex = Assert.Throws<InvalidIdentifierException>(() => identifier.ValidateIdentifier());
            Assert.AreEqual(IdentifierValidationError.InvalidCharacter, ex.ErrorType);
            Assert.AreEqual("invalid@Identifier", ex.Identifier);
        }

        /// <summary>
        /// Verifies that ValidateIdentifier throws InvalidIdentifierException for reserved keyword.
        /// </summary>
        [Test]
        public void ValidateIdentifier_ReservedKeyword_ThrowsInvalidIdentifierExceptionWithReservedKeywordError()
        {
            // Arrange
            string identifier = "class";

            // Act & Assert
            var ex = Assert.Throws<InvalidIdentifierException>(() => identifier.ValidateIdentifier());
            Assert.AreEqual(IdentifierValidationError.ReservedKeyword, ex.ErrorType);
            Assert.AreEqual("class", ex.Identifier);
        }

        /// <summary>
        /// Verifies that ValidateIdentifier exception message contains the identifier for invalid character error.
        /// </summary>
        [Test]
        public void ValidateIdentifier_ContainingHyphen_ExceptionMessageContainsIdentifier()
        {
            // Arrange
            string identifier = "my-invalid";

            // Act & Assert
            var ex = Assert.Throws<InvalidIdentifierException>(() => identifier.ValidateIdentifier());
            Assert.That(ex.Message, Does.Contain("my-invalid"));
        }

        #endregion

        #region Edge Cases

        /// <summary>
        /// Verifies that IsValidIdentifier handles Unicode letters correctly.
        /// </summary>
        [Test]
        public void IsValidIdentifier_UnicodeLetters_ReturnsTrue()
        {
            // Arrange
            string identifier = "变量";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier handles very long identifiers.
        /// </summary>
        [Test]
        public void IsValidIdentifier_VeryLongIdentifier_ReturnsTrue()
        {
            // Arrange
            string identifier = "this_is_a_very_long_identifier_name_that_should_still_be_valid_as_long_as_it_follows_the_rules";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier is case-sensitive for keywords.
        /// </summary>
        [Test]
        public void IsValidIdentifier_KeywordDifferentCase_ReturnsTrue()
        {
            // Arrange
            string identifier = "Class"; // Capitalized, not a keyword

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(error);
        }

        /// <summary>
        /// Verifies that IsValidIdentifier handles identifier with consecutive underscores.
        /// </summary>
        [Test]
        public void IsValidIdentifier_ConsecutiveUnderscores_ReturnsTrue()
        {
            // Arrange
            string identifier = "my__variable___name";

            // Act
            bool result = identifier.IsValidIdentifier(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(error);
        }

        #endregion
    }
}
