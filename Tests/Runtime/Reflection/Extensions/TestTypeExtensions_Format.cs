using System;
using System.Collections.Generic;
using NUnit.Framework;
using EasyToolkit.Core.Reflection;

namespace EasyToolkit.Core.Reflection.Tests
{
    /// <summary>
    /// Tests for <see cref="TypeExtensions.ToCodeString(Type)"/> and <see cref="TypeExtensions.ToCodeString(Type, TypeFormat)"/>.
    /// </summary>
    public class TestTypeExtensions_Format
    {
        #region Test Helper Classes

        public class NestedTestClass
        {
            public class DeepNestedClass
            {
            }
        }

        public class GenericTestClass<T>
        {
        }

        public class MultiGenericTestClass<TKey, TValue>
        {
        }

        #endregion

        #region Primitive Types - Type Aliases

        /// <summary>
        /// Verifies that ToCodeString with default format returns "void" for void type.
        /// </summary>
        [Test]
        public void ToCodeString_VoidType_ReturnsVoidAlias()
        {
            // Arrange
            var type = typeof(void);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("void"));
        }

        /// <summary>
        /// Verifies that ToCodeString with default format returns "int" for Int32 type.
        /// </summary>
        [Test]
        public void ToCodeString_Int32Type_ReturnsIntAlias()
        {
            // Arrange
            var type = typeof(int);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("int"));
        }

        /// <summary>
        /// Verifies that ToCodeString with default format returns "string" for String type.
        /// </summary>
        [Test]
        public void ToCodeString_StringType_ReturnsStringAlias()
        {
            // Arrange
            var type = typeof(string);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("string"));
        }

        /// <summary>
        /// Verifies that ToCodeString with default format returns "bool" for Boolean type.
        /// </summary>
        [Test]
        public void ToCodeString_BoolType_ReturnsBoolAlias()
        {
            // Arrange
            var type = typeof(bool);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("bool"));
        }

        /// <summary>
        /// Verifies that ToCodeString with default format returns "object" for Object type.
        /// </summary>
        [Test]
        public void ToCodeString_ObjectType_ReturnsObjectAlias()
        {
            // Arrange
            var type = typeof(object);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("object"));
        }

        /// <summary>
        /// Verifies all primitive type aliases are correctly returned.
        /// </summary>
        [TestCase(typeof(byte), "byte")]
        [TestCase(typeof(sbyte), "sbyte")]
        [TestCase(typeof(char), "char")]
        [TestCase(typeof(decimal), "decimal")]
        [TestCase(typeof(double), "double")]
        [TestCase(typeof(float), "float")]
        [TestCase(typeof(uint), "uint")]
        [TestCase(typeof(long), "long")]
        [TestCase(typeof(ulong), "ulong")]
        [TestCase(typeof(short), "short")]
        [TestCase(typeof(ushort), "ushort")]
        public void ToCodeString_PrimitiveTypes_ReturnsCorrectAliases(Type type, string expectedAlias)
        {
            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo(expectedAlias));
        }

        #endregion

        #region Format Options - None

        /// <summary>
        /// Verifies that ToCodeString with None format returns type name without namespace for primitive types.
        /// </summary>
        [Test]
        public void ToCodeString_NoneFormatOnInt32_ReturnsInt32()
        {
            // Arrange
            var type = typeof(int);

            // Act
            var result = type.ToCodeString(TypeFormat.None);

            // Assert
            Assert.That(result, Is.EqualTo("Int32"));
        }

        /// <summary>
        /// Verifies that ToCodeString with None format returns simple type name without namespace.
        /// </summary>
        [Test]
        public void ToCodeString_NoneFormatOnList_ReturnsListWithoutNamespace()
        {
            // Arrange
            var type = typeof(List<int>);

            // Act
            var result = type.ToCodeString(TypeFormat.None);

            // Assert
            Assert.That(result, Is.EqualTo("List<Int32>"));
        }

        #endregion

        #region Format Options - IncludeNamespace

        /// <summary>
        /// Verifies that ToCodeString with IncludeNamespace format includes full namespace.
        /// </summary>
        [Test]
        public void ToCodeString_IncludeNamespaceOnInt32_ReturnsFullName()
        {
            // Arrange
            var type = typeof(int);

            // Act
            var result = type.ToCodeString(TypeFormat.IncludeNamespace);

            // Assert
            Assert.That(result, Is.EqualTo("System.Int32"));
        }

        /// <summary>
        /// Verifies that ToCodeString with IncludeNamespace format includes namespace for generic types.
        /// </summary>
        [Test]
        public void ToCodeString_IncludeNamespaceOnList_ReturnsFullNamespace()
        {
            // Arrange
            var type = typeof(List<int>);

            // Act
            var result = type.ToCodeString(TypeFormat.IncludeNamespace);

            // Assert
            Assert.That(result, Is.EqualTo("System.Collections.Generic.List<System.Int32>"));
        }

        /// <summary>
        /// Verifies that ToCodeString with both flags includes namespace and uses aliases.
        /// </summary>
        [Test]
        public void ToCodeString_FullFormatOnList_ReturnsNamespaceWithAliases()
        {
            // Arrange
            var type = typeof(List<int>);

            // Act
            var result = type.ToCodeString(TypeFormat.UseTypeAliases | TypeFormat.IncludeNamespace);

            // Assert
            Assert.That(result, Is.EqualTo("System.Collections.Generic.List<int>"));
        }

        #endregion

        #region Generic Types

        /// <summary>
        /// Verifies that ToCodeString formats single generic type correctly.
        /// </summary>
        [Test]
        public void ToCodeString_GenericTypeWithSingleArgument_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(List<string>);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("List<string>"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats multiple generic type arguments correctly.
        /// </summary>
        [Test]
        public void ToCodeString_GenericTypeWithMultipleArguments_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(Dictionary<string, int>);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("Dictionary<string, int>"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats nested generic types correctly.
        /// </summary>
        [Test]
        public void ToCodeString_NestedGenericTypes_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(List<List<int>>);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("List<List<int>>"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats complex nested generic types correctly.
        /// </summary>
        [Test]
        public void ToCodeString_ComplexNestedGenericTypes_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(Dictionary<string, List<int>>);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("Dictionary<string, List<int>>"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats open generic types correctly.
        /// </summary>
        [Test]
        public void ToCodeString_OpenGenericType_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(List<>);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("List<T>"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats multi-argument open generic types correctly.
        /// </summary>
        [Test]
        public void ToCodeString_OpenGenericWithMultipleArguments_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(Dictionary<,>);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("Dictionary<TKey, TValue>"));
        }

        #endregion

        #region Array Types

        /// <summary>
        /// Verifies that ToCodeString formats single-dimensional array correctly.
        /// </summary>
        [Test]
        public void ToCodeString_SingleDimensionalArray_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(int[]);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("int[]"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats two-dimensional array correctly.
        /// </summary>
        [Test]
        public void ToCodeString_TwoDimensionalArray_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(int[,]);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("int[,]"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats three-dimensional array correctly.
        /// </summary>
        [Test]
        public void ToCodeString_ThreeDimensionalArray_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(int[,,]);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("int[,,]"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats array of generic types correctly.
        /// </summary>
        [Test]
        public void ToCodeString_GenericTypeArray_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(List<int>[]);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("List<int>[]"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats jagged arrays correctly.
        /// </summary>
        [Test]
        public void ToCodeString_JaggedArray_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(int[][]);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("int[][]"));
        }

        #endregion

        #region Nested Types

        /// <summary>
        /// Verifies that ToCodeString formats nested class correctly.
        /// </summary>
        [Test]
        public void ToCodeString_NestedClass_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(NestedTestClass.DeepNestedClass);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("NestedTestClass"));
            Assert.That(result, Does.Contain("DeepNestedClass"));
        }

        /// <summary>
        /// Verifies that ToCodeString with IncludeNamespace formats nested class correctly.
        /// </summary>
        [Test]
        public void ToCodeString_NestedClassWithNamespace_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(NestedTestClass.DeepNestedClass);

            // Act
            var result = type.ToCodeString(TypeFormat.IncludeNamespace);

            // Assert
            Assert.That(result, Does.Contain("EasyToolkit.Core.Reflection.Tests"));
        }

        #endregion

        #region Edge Cases

        /// <summary>
        /// Verifies that ToCodeString throws ArgumentNullException when type is null.
        /// </summary>
        [Test]
        public void ToCodeString_NullType_ThrowsArgumentNullException()
        {
            // Arrange
            Type type = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => type.ToCodeString());
            Assert.That(ex.ParamName, Is.EqualTo("type"));
        }

        /// <summary>
        /// Verifies that ToCodeString with format throws ArgumentNullException when type is null.
        /// </summary>
        [Test]
        public void ToCodeString_WithFormatNullType_ThrowsArgumentNullException()
        {
            // Arrange
            Type type = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => type.ToCodeString(TypeFormat.Default));
            Assert.That(ex.ParamName, Is.EqualTo("type"));
        }

        /// <summary>
        /// Verifies that ToCodeString handles custom class types correctly.
        /// </summary>
        [Test]
        public void ToCodeString_CustomClassType_ReturnsTypeName()
        {
            // Arrange
            var type = typeof(TestTypeExtensions_Format);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("TestTypeExtensions_Format"));
        }

        /// <summary>
        /// Verifies that ToCodeString handles enum types correctly.
        /// </summary>
        [Test]
        public void ToCodeString_EnumType_ReturnsTypeName()
        {
            // Arrange
            var type = typeof(TypeFormat);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("TypeFormat"));
        }

        /// <summary>
        /// Verifies that ToCodeString handles delegate types correctly.
        /// </summary>
        [Test]
        public void ToCodeString_DelegateType_ReturnsCorrectFormat()
        {
            // Arrange
            var type = typeof(Func<int, string>);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("Func<int, string>"));
        }

        /// <summary>
        /// Verifies that ToCodeString handles interface types correctly.
        /// </summary>
        [Test]
        public void ToCodeString_InterfaceType_ReturnsTypeName()
        {
            // Arrange
            var type = typeof(IList<int>);

            // Act
            var result = type.ToCodeString();

            // Assert
            Assert.That(result, Is.EqualTo("IList<int>"));
        }

        #endregion
    }
}
