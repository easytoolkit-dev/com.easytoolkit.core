using NUnit.Framework;

namespace EasyToolkit.Core.Textual.Tests
{
    /// <summary>
    /// Unit tests for StringNamingExtensions.
    /// </summary>
    public class TestStringNamingExtensions
    {
        #region ToPascalCase Tests

        /// <summary>
        /// Verifies that ToPascalCase converts camelCase to PascalCase correctly.
        /// </summary>
        [Test]
        public void ToPascalCase_CamelCaseInput_ReturnsPascalCase()
        {
            // Arrange
            string input = "myVariableName";

            // Act
            string result = input.ToPascalCase();

            // Assert
            Assert.AreEqual("MyVariableName", result);
        }

        /// <summary>
        /// Verifies that ToPascalCase converts snake_case to PascalCase correctly.
        /// </summary>
        [Test]
        public void ToPascalCase_SnakeCaseInput_ReturnsPascalCase()
        {
            // Arrange
            string input = "my_variable_name";

            // Act
            string result = input.ToPascalCase();

            // Assert
            Assert.AreEqual("MyVariableName", result);
        }

        /// <summary>
        /// Verifies that ToPascalCase converts kebab-case to PascalCase correctly.
        /// </summary>
        [Test]
        public void ToPascalCase_KebabCaseInput_ReturnsPascalCase()
        {
            // Arrange
            string input = "my-variable-name";

            // Act
            string result = input.ToPascalCase();

            // Assert
            Assert.AreEqual("MyVariableName", result);
        }

        /// <summary>
        /// Verifies that ToPascalCase handles consecutive uppercase letters correctly.
        /// </summary>
        [Test]
        public void ToPascalCase_ConsecutiveUppercaseLetters_HandlesCorrectly()
        {
            // Arrange
            string input = "XMLParser";

            // Act
            string result = input.ToPascalCase();

            // Assert
            Assert.AreEqual("XmlParser", result);
        }

        /// <summary>
        /// Verifies that ToPascalCase handles SCREAMING_SNAKE_CASE correctly.
        /// </summary>
        [Test]
        public void ToPascalCase_ScreamingSnakeCaseInput_ReturnsPascalCase()
        {
            // Arrange
            string input = "MY_VARIABLE_NAME";

            // Act
            string result = input.ToPascalCase();

            // Assert
            Assert.AreEqual("MyVariableName", result);
        }

        /// <summary>
        /// Verifies that ToPascalCase handles SCREAMING-KEBAB-CASE correctly.
        /// </summary>
        [Test]
        public void ToPascalCase_ScreamingKebabCaseInput_ReturnsPascalCase()
        {
            // Arrange
            string input = "MY-VARIABLE-NAME";

            // Act
            string result = input.ToPascalCase();

            // Assert
            Assert.AreEqual("MyVariableName", result);
        }

        /// <summary>
        /// Verifies that ToPascalCase handles single word input.
        /// </summary>
        [Test]
        public void ToPascalCase_SingleWordInput_CapitalizesFirstLetter()
        {
            // Arrange
            string input = "hello";

            // Act
            string result = input.ToPascalCase();

            // Assert
            Assert.AreEqual("Hello", result);
        }

        /// <summary>
        /// Verifies that ToPascalCase returns empty string for null input.
        /// </summary>
        [Test]
        public void ToPascalCase_NullInput_ReturnsEmptyString()
        {
            // Arrange
            string input = null;

            // Act
            string result = input.ToPascalCase();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Verifies that ToPascalCase returns empty string for empty input.
        /// </summary>
        [Test]
        public void ToPascalCase_EmptyInput_ReturnsEmptyString()
        {
            // Arrange
            string input = string.Empty;

            // Act
            string result = input.ToPascalCase();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Verifies that ToPascalCase handles input with mixed delimiters.
        /// </summary>
        [Test]
        public void ToPascalCase_MixedDelimiters_HandlesCorrectly()
        {
            // Arrange
            string input = "my_variable-name_test";

            // Act
            string result = input.ToPascalCase();

            // Assert
            Assert.AreEqual("MyVariableNameTest", result);
        }

        #endregion

        #region ToCamelCase Tests

        /// <summary>
        /// Verifies that ToCamelCase converts PascalCase to camelCase correctly.
        /// </summary>
        [Test]
        public void ToCamelCase_PascalCaseInput_ReturnsCamelCase()
        {
            // Arrange
            string input = "MyVariableName";

            // Act
            string result = input.ToCamelCase();

            // Assert
            Assert.AreEqual("myVariableName", result);
        }

        /// <summary>
        /// Verifies that ToCamelCase converts snake_case to camelCase correctly.
        /// </summary>
        [Test]
        public void ToCamelCase_SnakeCaseInput_ReturnsCamelCase()
        {
            // Arrange
            string input = "my_variable_name";

            // Act
            string result = input.ToCamelCase();

            // Assert
            Assert.AreEqual("myVariableName", result);
        }

        /// <summary>
        /// Verifies that ToCamelCase converts kebab-case to camelCase correctly.
        /// </summary>
        [Test]
        public void ToCamelCase_KebabCaseInput_ReturnsCamelCase()
        {
            // Arrange
            string input = "my-variable-name";

            // Act
            string result = input.ToCamelCase();

            // Assert
            Assert.AreEqual("myVariableName", result);
        }

        /// <summary>
        /// Verifies that ToCamelCase handles consecutive uppercase letters correctly.
        /// </summary>
        [Test]
        public void ToCamelCase_ConsecutiveUppercaseLetters_HandlesCorrectly()
        {
            // Arrange
            string input = "XMLParser";

            // Act
            string result = input.ToCamelCase();

            // Assert
            Assert.AreEqual("xmlParser", result);
        }

        /// <summary>
        /// Verifies that ToCamelCase handles SCREAMING_SNAKE_CASE correctly.
        /// </summary>
        [Test]
        public void ToCamelCase_ScreamingSnakeCaseInput_ReturnsCamelCase()
        {
            // Arrange
            string input = "MY_VARIABLE_NAME";

            // Act
            string result = input.ToCamelCase();

            // Assert
            Assert.AreEqual("myVariableName", result);
        }

        /// <summary>
        /// Verifies that ToCamelCase handles single word input.
        /// </summary>
        [Test]
        public void ToCamelCase_SingleWordInput_LowercasesFirstLetter()
        {
            // Arrange
            string input = "Hello";

            // Act
            string result = input.ToCamelCase();

            // Assert
            Assert.AreEqual("hello", result);
        }

        /// <summary>
        /// Verifies that ToCamelCase returns empty string for null input.
        /// </summary>
        [Test]
        public void ToCamelCase_NullInput_ReturnsEmptyString()
        {
            // Arrange
            string input = null;

            // Act
            string result = input.ToCamelCase();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Verifies that ToCamelCase returns empty string for empty input.
        /// </summary>
        [Test]
        public void ToCamelCase_EmptyInput_ReturnsEmptyString()
        {
            // Arrange
            string input = string.Empty;

            // Act
            string result = input.ToCamelCase();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        #endregion

        #region ToSnakeCase Tests

        /// <summary>
        /// Verifies that ToSnakeCase converts PascalCase to snake_case correctly.
        /// </summary>
        [Test]
        public void ToSnakeCase_PascalCaseInput_ReturnsSnakeCase()
        {
            // Arrange
            string input = "MyVariableName";

            // Act
            string result = input.ToSnakeCase();

            // Assert
            Assert.AreEqual("my_variable_name", result);
        }

        /// <summary>
        /// Verifies that ToSnakeCase converts camelCase to snake_case correctly.
        /// </summary>
        [Test]
        public void ToSnakeCase_CamelCaseInput_ReturnsSnakeCase()
        {
            // Arrange
            string input = "myVariableName";

            // Act
            string result = input.ToSnakeCase();

            // Assert
            Assert.AreEqual("my_variable_name", result);
        }

        /// <summary>
        /// Verifies that ToSnakeCase converts kebab-case to snake_case correctly.
        /// </summary>
        [Test]
        public void ToSnakeCase_KebabCaseInput_ReturnsSnakeCase()
        {
            // Arrange
            string input = "my-variable-name";

            // Act
            string result = input.ToSnakeCase();

            // Assert
            Assert.AreEqual("my_variable_name", result);
        }

        /// <summary>
        /// Verifies that ToSnakeCase handles consecutive uppercase letters correctly.
        /// </summary>
        [Test]
        public void ToSnakeCase_ConsecutiveUppercaseLetters_HandlesCorrectly()
        {
            // Arrange
            string input = "XMLParser";

            // Act
            string result = input.ToSnakeCase();

            // Assert
            Assert.AreEqual("xml_parser", result);
        }

        /// <summary>
        /// Verifies that ToSnakeCase handles SCREAMING_SNAKE_CASE correctly.
        /// </summary>
        [Test]
        public void ToSnakeCase_ScreamingSnakeCaseInput_ReturnsSnakeCase()
        {
            // Arrange
            string input = "MY_VARIABLE_NAME";

            // Act
            string result = input.ToSnakeCase();

            // Assert
            Assert.AreEqual("my_variable_name", result);
        }

        /// <summary>
        /// Verifies that ToSnakeCase handles single word input.
        /// </summary>
        [Test]
        public void ToSnakeCase_SingleWordInput_ReturnsLowercase()
        {
            // Arrange
            string input = "Hello";

            // Act
            string result = input.ToSnakeCase();

            // Assert
            Assert.AreEqual("hello", result);
        }

        /// <summary>
        /// Verifies that ToSnakeCase returns empty string for null input.
        /// </summary>
        [Test]
        public void ToSnakeCase_NullInput_ReturnsEmptyString()
        {
            // Arrange
            string input = null;

            // Act
            string result = input.ToSnakeCase();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Verifies that ToSnakeCase returns empty string for empty input.
        /// </summary>
        [Test]
        public void ToSnakeCase_EmptyInput_ReturnsEmptyString()
        {
            // Arrange
            string input = string.Empty;

            // Act
            string result = input.ToSnakeCase();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        #endregion

        #region ToScreamingSnakeCase Tests

        /// <summary>
        /// Verifies that ToScreamingSnakeCase converts PascalCase to SCREAMING_SNAKE_CASE correctly.
        /// </summary>
        [Test]
        public void ToScreamingSnakeCase_PascalCaseInput_ReturnsScreamingSnakeCase()
        {
            // Arrange
            string input = "MyVariableName";

            // Act
            string result = input.ToScreamingSnakeCase();

            // Assert
            Assert.AreEqual("MY_VARIABLE_NAME", result);
        }

        /// <summary>
        /// Verifies that ToScreamingSnakeCase converts camelCase to SCREAMING_SNAKE_CASE correctly.
        /// </summary>
        [Test]
        public void ToScreamingSnakeCase_CamelCaseInput_ReturnsScreamingSnakeCase()
        {
            // Arrange
            string input = "myVariableName";

            // Act
            string result = input.ToScreamingSnakeCase();

            // Assert
            Assert.AreEqual("MY_VARIABLE_NAME", result);
        }

        /// <summary>
        /// Verifies that ToScreamingSnakeCase converts kebab-case to SCREAMING_SNAKE_CASE correctly.
        /// </summary>
        [Test]
        public void ToScreamingSnakeCase_KebabCaseInput_ReturnsScreamingSnakeCase()
        {
            // Arrange
            string input = "my-variable-name";

            // Act
            string result = input.ToScreamingSnakeCase();

            // Assert
            Assert.AreEqual("MY_VARIABLE_NAME", result);
        }

        /// <summary>
        /// Verifies that ToScreamingSnakeCase handles consecutive uppercase letters correctly.
        /// </summary>
        [Test]
        public void ToScreamingSnakeCase_ConsecutiveUppercaseLetters_HandlesCorrectly()
        {
            // Arrange
            string input = "XMLParser";

            // Act
            string result = input.ToScreamingSnakeCase();

            // Assert
            Assert.AreEqual("XML_PARSER", result);
        }

        /// <summary>
        /// Verifies that ToScreamingSnakeCase handles snake_case correctly.
        /// </summary>
        [Test]
        public void ToScreamingSnakeCase_SnakeCaseInput_ReturnsScreamingSnakeCase()
        {
            // Arrange
            string input = "my_variable_name";

            // Act
            string result = input.ToScreamingSnakeCase();

            // Assert
            Assert.AreEqual("MY_VARIABLE_NAME", result);
        }

        /// <summary>
        /// Verifies that ToScreamingSnakeCase handles single word input.
        /// </summary>
        [Test]
        public void ToScreamingSnakeCase_SingleWordInput_ReturnsUppercase()
        {
            // Arrange
            string input = "Hello";

            // Act
            string result = input.ToScreamingSnakeCase();

            // Assert
            Assert.AreEqual("HELLO", result);
        }

        /// <summary>
        /// Verifies that ToScreamingSnakeCase returns empty string for null input.
        /// </summary>
        [Test]
        public void ToScreamingSnakeCase_NullInput_ReturnsEmptyString()
        {
            // Arrange
            string input = null;

            // Act
            string result = input.ToScreamingSnakeCase();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Verifies that ToScreamingSnakeCase returns empty string for empty input.
        /// </summary>
        [Test]
        public void ToScreamingSnakeCase_EmptyInput_ReturnsEmptyString()
        {
            // Arrange
            string input = string.Empty;

            // Act
            string result = input.ToScreamingSnakeCase();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        #endregion

        #region ToKebabCase Tests

        /// <summary>
        /// Verifies that ToKebabCase converts PascalCase to kebab-case correctly.
        /// </summary>
        [Test]
        public void ToKebabCase_PascalCaseInput_ReturnsKebabCase()
        {
            // Arrange
            string input = "MyVariableName";

            // Act
            string result = input.ToKebabCase();

            // Assert
            Assert.AreEqual("my-variable-name", result);
        }

        /// <summary>
        /// Verifies that ToKebabCase converts camelCase to kebab-case correctly.
        /// </summary>
        [Test]
        public void ToKebabCase_CamelCaseInput_ReturnsKebabCase()
        {
            // Arrange
            string input = "myVariableName";

            // Act
            string result = input.ToKebabCase();

            // Assert
            Assert.AreEqual("my-variable-name", result);
        }

        /// <summary>
        /// Verifies that ToKebabCase converts snake_case to kebab-case correctly.
        /// </summary>
        [Test]
        public void ToKebabCase_SnakeCaseInput_ReturnsKebabCase()
        {
            // Arrange
            string input = "my_variable_name";

            // Act
            string result = input.ToKebabCase();

            // Assert
            Assert.AreEqual("my-variable-name", result);
        }

        /// <summary>
        /// Verifies that ToKebabCase handles consecutive uppercase letters correctly.
        /// </summary>
        [Test]
        public void ToKebabCase_ConsecutiveUppercaseLetters_HandlesCorrectly()
        {
            // Arrange
            string input = "XMLParser";

            // Act
            string result = input.ToKebabCase();

            // Assert
            Assert.AreEqual("xml-parser", result);
        }

        /// <summary>
        /// Verifies that ToKebabCase handles SCREAMING-KEBAB-CASE correctly.
        /// </summary>
        [Test]
        public void ToKebabCase_ScreamingKebabCaseInput_ReturnsKebabCase()
        {
            // Arrange
            string input = "MY-VARIABLE-NAME";

            // Act
            string result = input.ToKebabCase();

            // Assert
            Assert.AreEqual("my-variable-name", result);
        }

        /// <summary>
        /// Verifies that ToKebabCase handles single word input.
        /// </summary>
        [Test]
        public void ToKebabCase_SingleWordInput_ReturnsLowercase()
        {
            // Arrange
            string input = "Hello";

            // Act
            string result = input.ToKebabCase();

            // Assert
            Assert.AreEqual("hello", result);
        }

        /// <summary>
        /// Verifies that ToKebabCase returns empty string for null input.
        /// </summary>
        [Test]
        public void ToKebabCase_NullInput_ReturnsEmptyString()
        {
            // Arrange
            string input = null;

            // Act
            string result = input.ToKebabCase();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Verifies that ToKebabCase returns empty string for empty input.
        /// </summary>
        [Test]
        public void ToKebabCase_EmptyInput_ReturnsEmptyString()
        {
            // Arrange
            string input = string.Empty;

            // Act
            string result = input.ToKebabCase();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        #endregion

        #region ToScreamingKebabCase Tests

        /// <summary>
        /// Verifies that ToScreamingKebabCase converts PascalCase to SCREAMING-KEBAB-CASE correctly.
        /// </summary>
        [Test]
        public void ToScreamingKebabCase_PascalCaseInput_ReturnsScreamingKebabCase()
        {
            // Arrange
            string input = "MyVariableName";

            // Act
            string result = input.ToScreamingKebabCase();

            // Assert
            Assert.AreEqual("MY-VARIABLE-NAME", result);
        }

        /// <summary>
        /// Verifies that ToScreamingKebabCase converts camelCase to SCREAMING-KEBAB-CASE correctly.
        /// </summary>
        [Test]
        public void ToScreamingKebabCase_CamelCaseInput_ReturnsScreamingKebabCase()
        {
            // Arrange
            string input = "myVariableName";

            // Act
            string result = input.ToScreamingKebabCase();

            // Assert
            Assert.AreEqual("MY-VARIABLE-NAME", result);
        }

        /// <summary>
        /// Verifies that ToScreamingKebabCase converts snake_case to SCREAMING-KEBAB-CASE correctly.
        /// </summary>
        [Test]
        public void ToScreamingKebabCase_SnakeCaseInput_ReturnsScreamingKebabCase()
        {
            // Arrange
            string input = "my_variable_name";

            // Act
            string result = input.ToScreamingKebabCase();

            // Assert
            Assert.AreEqual("MY-VARIABLE-NAME", result);
        }

        /// <summary>
        /// Verifies that ToScreamingKebabCase handles consecutive uppercase letters correctly.
        /// </summary>
        [Test]
        public void ToScreamingKebabCase_ConsecutiveUppercaseLetters_HandlesCorrectly()
        {
            // Arrange
            string input = "XMLParser";

            // Act
            string result = input.ToScreamingKebabCase();

            // Assert
            Assert.AreEqual("XML-PARSER", result);
        }

        /// <summary>
        /// Verifies that ToScreamingKebabCase handles kebab-case correctly.
        /// </summary>
        [Test]
        public void ToScreamingKebabCase_KebabCaseInput_ReturnsScreamingKebabCase()
        {
            // Arrange
            string input = "my-variable-name";

            // Act
            string result = input.ToScreamingKebabCase();

            // Assert
            Assert.AreEqual("MY-VARIABLE-NAME", result);
        }

        /// <summary>
        /// Verifies that ToScreamingKebabCase handles single word input.
        /// </summary>
        [Test]
        public void ToScreamingKebabCase_SingleWordInput_ReturnsUppercase()
        {
            // Arrange
            string input = "Hello";

            // Act
            string result = input.ToScreamingKebabCase();

            // Assert
            Assert.AreEqual("HELLO", result);
        }

        /// <summary>
        /// Verifies that ToScreamingKebabCase returns empty string for null input.
        /// </summary>
        [Test]
        public void ToScreamingKebabCase_NullInput_ReturnsEmptyString()
        {
            // Arrange
            string input = null;

            // Act
            string result = input.ToScreamingKebabCase();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        /// <summary>
        /// Verifies that ToScreamingKebabCase returns empty string for empty input.
        /// </summary>
        [Test]
        public void ToScreamingKebabCase_EmptyInput_ReturnsEmptyString()
        {
            // Arrange
            string input = string.Empty;

            // Act
            string result = input.ToScreamingKebabCase();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        #endregion

        #region Edge Case Tests

        /// <summary>
        /// Verifies that conversion methods handle input with only delimiters correctly.
        /// </summary>
        [Test]
        public void ConversionMethods_OnlyDelimiters_ReturnsEmptyString()
        {
            // Arrange
            string input = "___---";

            // Act & Assert
            Assert.AreEqual(string.Empty, input.ToPascalCase());
            Assert.AreEqual(string.Empty, input.ToCamelCase());
            Assert.AreEqual(string.Empty, input.ToSnakeCase());
            Assert.AreEqual(string.Empty, input.ToScreamingSnakeCase());
            Assert.AreEqual(string.Empty, input.ToKebabCase());
            Assert.AreEqual(string.Empty, input.ToScreamingKebabCase());
        }

        /// <summary>
        /// Verifies that conversion methods handle single character input correctly.
        /// </summary>
        [Test]
        public void ConversionMethods_SingleCharacter_HandlesCorrectly()
        {
            // Arrange
            string input = "a";

            // Act & Assert
            Assert.AreEqual("A", input.ToPascalCase());
            Assert.AreEqual("a", input.ToCamelCase());
            Assert.AreEqual("a", input.ToSnakeCase());
            Assert.AreEqual("A", input.ToScreamingSnakeCase());
            Assert.AreEqual("a", input.ToKebabCase());
            Assert.AreEqual("A", input.ToScreamingKebabCase());
        }

        /// <summary>
        /// Verifies that conversion methods preserve numbers correctly.
        /// </summary>
        [Test]
        public void ConversionMethods_WithNumbers_PreservesNumbers()
        {
            // Arrange
            string input = "myVariable123Name";

            // Act & Assert
            Assert.AreEqual("MyVariable123Name", input.ToPascalCase());
            Assert.AreEqual("myVariable123Name", input.ToCamelCase());
            Assert.AreEqual("my_variable123_name", input.ToSnakeCase());
            Assert.AreEqual("my-variable123-name", input.ToKebabCase());
        }

        /// <summary>
        /// Verifies that conversion methods round-trip correctly for same convention.
        /// </summary>
        [Test]
        public void ConversionMethods_RoundTrip_SameConvention()
        {
            // Arrange
            string pascalCase = "MyVariableName";

            // Act & Assert - PascalCase to PascalCase
            Assert.AreEqual(pascalCase, pascalCase.ToPascalCase());

            // Arrange
            string camelCase = "myVariableName";

            // Act & Assert - camelCase to camelCase
            Assert.AreEqual(camelCase, camelCase.ToCamelCase());

            // Arrange
            string snakeCase = "my_variable_name";

            // Act & Assert - snake_case to snake_case
            Assert.AreEqual(snakeCase, snakeCase.ToSnakeCase());

            // Arrange
            string kebabCase = "my-variable-name";

            // Act & Assert - kebab-case to kebab-case
            Assert.AreEqual(kebabCase, kebabCase.ToKebabCase());
        }

        #endregion
    }
}
