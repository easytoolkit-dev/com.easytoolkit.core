using System;
using System.Collections.Generic;
using NUnit.Framework;
using EasyToolkit.Core.Reflection;

namespace EasyToolkit.Core.Reflection.Tests
{
    /// <summary>
    /// Tests for <see cref="TypeExtensions"/> open generic type extension methods.
    /// </summary>
    [TestFixture]
    public class TestTypeExtensions_OpenGenericType
    {
        #region Test Types

        /// <summary>
        /// Simple generic type with one parameter.
        /// </summary>
        public class SimpleContainer<T>
        {
            public T Value { get; set; }
        }

        /// <summary>
        /// Generic type with two parameters.
        /// </summary>
        public class DoubleContainer<T, U>
        {
            public T First { get; set; }
            public U Second { get; set; }
        }

        /// <summary>
        /// Generic type with class constraint.
        /// </summary>
        public class ClassConstrainedContainer<T> where T : class
        {
            public T Value { get; set; }
        }

        /// <summary>
        /// Generic type with struct constraint.
        /// </summary>
        public class StructConstrainedContainer<T> where T : struct
        {
            public T Value { get; set; }
        }

        /// <summary>
        /// Generic type with multiple constraints.
        /// </summary>
        public class MultiConstrainedContainer<T> where T : class, new()
        {
            public T Value { get; set; }
        }

        /// <summary>
        /// Derived container for inheritance testing.
        /// </summary>
        public class DerivedContainer : SimpleContainer<int> { }

        /// <summary>
        /// Generic interface for testing.
        /// </summary>
        public interface IContainer<T>
        {
            T GetValue();
        }

        /// <summary>
        /// Class implementing generic interface.
        /// </summary>
        public class ContainerImpl<T> : IContainer<T>
        {
            public T Value { get; set; }
            public T GetValue() => Value;
        }

        #endregion

        #region GetGenericArgumentsRelativeTo

        /// <summary>
        /// Verifies that GetGenericArgumentsRelativeTo returns correct arguments for simple generic type.
        /// </summary>
        [Test]
        public void GetGenericArgumentsRelativeTo_SimpleGenericType_ReturnsCorrectArguments()
        {
            // Arrange
            var type = typeof(SimpleContainer<int>);

            // Act
            var arguments = type.GetGenericArgumentsRelativeTo(typeof(SimpleContainer<>));

            // Assert
            Assert.AreEqual(1, arguments.Length);
            Assert.AreEqual(typeof(int), arguments[0]);
        }

        /// <summary>
        /// Verifies that GetGenericArgumentsRelativeTo returns correct arguments for multi-parameter generic type.
        /// </summary>
        [Test]
        public void GetGenericArgumentsRelativeTo_MultiParameterGenericType_ReturnsCorrectArguments()
        {
            // Arrange
            var type = typeof(DoubleContainer<string, int>);

            // Act
            var arguments = type.GetGenericArgumentsRelativeTo(typeof(DoubleContainer<,>));

            // Assert
            Assert.AreEqual(2, arguments.Length);
            Assert.AreEqual(typeof(string), arguments[0]);
            Assert.AreEqual(typeof(int), arguments[1]);
        }

        /// <summary>
        /// Verifies that GetGenericArgumentsRelativeTo works with partially constructed type.
        /// </summary>
        [Test]
        public void GetGenericArgumentsRelativeTo_PartiallyConstructedType_ReturnsCorrectArguments()
        {
            // Arrange
            var dictTypeDef = typeof(Dictionary<,>);
            var tValueParam = dictTypeDef.GetGenericArguments()[1];
            var partiallyConstructed = dictTypeDef.MakeGenericType(typeof(string), tValueParam);

            // Act
            var arguments = partiallyConstructed.GetGenericArgumentsRelativeTo(typeof(Dictionary<,>));

            // Assert
            Assert.AreEqual(2, arguments.Length);
            Assert.AreEqual(typeof(string), arguments[0]);
            Assert.AreEqual(tValueParam, arguments[1]);
        }

        /// <summary>
        /// Verifies that GetGenericArgumentsRelativeTo works with inherited generic types.
        /// </summary>
        [Test]
        public void GetGenericArgumentsRelativeTo_InheritedGenericType_ReturnsMappedArguments()
        {
            // Arrange
            var derivedType = typeof(DerivedContainer);

            // Act
            var arguments = derivedType.GetGenericArgumentsRelativeTo(typeof(SimpleContainer<>));

            // Assert
            Assert.AreEqual(1, arguments.Length);
            Assert.AreEqual(typeof(int), arguments[0]);
        }

        /// <summary>
        /// Verifies that GetGenericArgumentsRelativeTo works with interface implementation.
        /// </summary>
        [Test]
        public void GetGenericArgumentsRelativeTo_InterfaceImplementation_ReturnsInterfaceArguments()
        {
            // Arrange
            var type = typeof(ContainerImpl<string>);

            // Act
            var arguments = type.GetGenericArgumentsRelativeTo(typeof(IContainer<>));

            // Assert
            Assert.AreEqual(1, arguments.Length);
            Assert.AreEqual(typeof(string), arguments[0]);
        }

        /// <summary>
        /// Verifies that GetGenericArgumentsRelativeTo throws ArgumentNullException when type is null.
        /// </summary>
        [Test]
        public void GetGenericArgumentsRelativeTo_NullType_ThrowsArgumentNullException()
        {
            // Arrange
            Type nullType = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => nullType.GetGenericArgumentsRelativeTo(typeof(SimpleContainer<>)));
            Assert.AreEqual("openGenericType", ex.ParamName);
        }

        /// <summary>
        /// Verifies that GetGenericArgumentsRelativeTo throws ArgumentNullException when definition is null.
        /// </summary>
        [Test]
        public void GetGenericArgumentsRelativeTo_NullDefinition_ThrowsArgumentNullException()
        {
            // Arrange
            var type = typeof(SimpleContainer<int>);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => type.GetGenericArgumentsRelativeTo(null));
            Assert.AreEqual("genericTypeDefinition", ex.ParamName);
        }

        /// <summary>
        /// Verifies that GetGenericArgumentsRelativeTo throws for unrelated type.
        /// </summary>
        [Test]
        public void GetGenericArgumentsRelativeTo_UnrelatedType_ThrowsInvalidOperationException()
        {
            // Arrange
            var type = typeof(SimpleContainer<int>);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => type.GetGenericArgumentsRelativeTo(typeof(List<>)));
        }

        #endregion

        #region GetGenericParameterCount

        /// <summary>
        /// Verifies that GetGenericParameterCount returns correct count for generic type definition.
        /// </summary>
        [Test]
        public void GetGenericParameterCount_GenericTypeDefinition_ReturnsCorrectCount()
        {
            // Arrange
            var type = typeof(SimpleContainer<>);

            // Act
            var count = type.GetGenericParameterCount();

            // Assert
            Assert.AreEqual(1, count);
        }

        /// <summary>
        /// Verifies that GetGenericParameterCount returns correct count for multi-parameter generic type.
        /// </summary>
        [Test]
        public void GetGenericParameterCount_MultiParameterGenericType_ReturnsCorrectCount()
        {
            // Arrange
            var type = typeof(DoubleContainer<,>);

            // Act
            var count = type.GetGenericParameterCount();

            // Assert
            Assert.AreEqual(2, count);
        }

        /// <summary>
        /// Verifies that GetGenericParameterCount returns 0 for fully constructed type.
        /// </summary>
        [Test]
        public void GetGenericParameterCount_FullyConstructedType_ReturnsZero()
        {
            // Arrange
            var type = typeof(SimpleContainer<int>);

            // Act
            var count = type.GetGenericParameterCount();

            // Assert
            Assert.AreEqual(0, count);
        }

        /// <summary>
        /// Verifies that GetGenericParameterCount returns correct count for partially constructed type.
        /// </summary>
        [Test]
        public void GetGenericParameterCount_PartiallyConstructedType_ReturnsUnresolvedCount()
        {
            // Arrange
            var dictTypeDef = typeof(Dictionary<,>);
            var tValueParam = dictTypeDef.GetGenericArguments()[1];
            var partiallyConstructed = dictTypeDef.MakeGenericType(typeof(string), tValueParam);

            // Act
            var count = partiallyConstructed.GetGenericParameterCount();

            // Assert
            Assert.AreEqual(1, count);
        }

        /// <summary>
        /// Verifies that GetGenericParameterCount throws ArgumentNullException when type is null.
        /// </summary>
        [Test]
        public void GetGenericParameterCount_NullType_ThrowsArgumentNullException()
        {
            // Arrange
            Type nullType = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => nullType.GetGenericParameterCount());
            Assert.AreEqual("openGenericType", ex.ParamName);
        }

        #endregion

        #region IsImplementsGenericDefinition

        /// <summary>
        /// Verifies that IsImplementsGenericDefinition returns true for the same generic type definition.
        /// </summary>
        [Test]
        public void IsImplementsGenericDefinition_SameType_ReturnsTrue()
        {
            // Arrange
            var type = typeof(SimpleContainer<>);

            // Act
            var result = type.IsImplementsGenericDefinition(typeof(SimpleContainer<>));

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifies that IsImplementsGenericDefinition returns true for base class inheritance.
        /// </summary>
        [Test]
        public void IsImplementsGenericDefinition_BaseClassInheritance_ReturnsTrue()
        {
            // Arrange
            var derivedType = typeof(DerivedContainer);

            // Act
            var result = derivedType.IsImplementsGenericDefinition(typeof(SimpleContainer<>));

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifies that IsImplementsGenericDefinition returns true for interface implementation.
        /// </summary>
        [Test]
        public void IsImplementsGenericDefinition_InterfaceImplementation_ReturnsTrue()
        {
            // Arrange
            var type = typeof(ContainerImpl<int>);

            // Act
            var result = type.IsImplementsGenericDefinition(typeof(IContainer<>));

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifies that IsImplementsGenericDefinition returns true for List implementing IEnumerable.
        /// </summary>
        [Test]
        public void IsImplementsGenericDefinition_ListImplementsIEnumerable_ReturnsTrue()
        {
            // Arrange
            var type = typeof(List<int>);

            // Act
            var result = type.IsImplementsGenericDefinition(typeof(IEnumerable<>));

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifies that IsImplementsGenericDefinition returns false for unrelated types.
        /// </summary>
        [Test]
        public void IsImplementsGenericDefinition_UnrelatedType_ReturnsFalse()
        {
            // Arrange
            var type = typeof(SimpleContainer<>);

            // Act
            var result = type.IsImplementsGenericDefinition(typeof(List<>));

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies that IsImplementsGenericDefinition throws ArgumentNullException when type is null.
        /// </summary>
        [Test]
        public void IsImplementsGenericDefinition_NullType_ThrowsArgumentNullException()
        {
            // Arrange
            Type nullType = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => nullType.IsImplementsGenericDefinition(typeof(SimpleContainer<>)));
            Assert.AreEqual("openGenericType", ex.ParamName);
        }

        /// <summary>
        /// Verifies that IsImplementsGenericDefinition throws ArgumentNullException when definition is null.
        /// </summary>
        [Test]
        public void IsImplementsGenericDefinition_NullDefinition_ThrowsArgumentNullException()
        {
            // Arrange
            var type = typeof(SimpleContainer<>);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => type.IsImplementsGenericDefinition(null));
            Assert.AreEqual("genericTypeDefinition", ex.ParamName);
        }

        /// <summary>
        /// Verifies that IsImplementsGenericDefinition throws ArgumentException for non-generic type definition.
        /// </summary>
        [Test]
        public void IsImplementsGenericDefinition_NonGenericTypeDefinition_ThrowsArgumentException()
        {
            // Arrange
            var type = typeof(SimpleContainer<>);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => type.IsImplementsGenericDefinition(typeof(string)));
            Assert.That(ex.Message, Does.Contain("generic type definition"));
        }

        #endregion

        #region SatisfiesConstraints

        /// <summary>
        /// Verifies that SatisfiesConstraints returns true when type arguments satisfy class constraint.
        /// </summary>
        [Test]
        public void SatisfiesConstraints_ClassConstraintWithValidType_ReturnsTrue()
        {
            // Arrange
            var type = typeof(ClassConstrainedContainer<>);

            // Act
            var result = type.SatisfiesConstraints(typeof(string));

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifies that SatisfiesConstraints returns false when type arguments violate class constraint.
        /// </summary>
        [Test]
        public void SatisfiesConstraints_ClassConstraintWithStructType_ReturnsFalse()
        {
            // Arrange
            var type = typeof(ClassConstrainedContainer<>);

            // Act
            var result = type.SatisfiesConstraints(typeof(int));

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies that SatisfiesConstraints returns true when type arguments satisfy struct constraint.
        /// </summary>
        [Test]
        public void SatisfiesConstraints_StructConstraintWithValidType_ReturnsTrue()
        {
            // Arrange
            var type = typeof(StructConstrainedContainer<>);

            // Act
            var result = type.SatisfiesConstraints(typeof(int));

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifies that SatisfiesConstraints returns false when type arguments violate struct constraint.
        /// </summary>
        [Test]
        public void SatisfiesConstraints_StructConstraintWithClassType_ReturnsFalse()
        {
            // Arrange
            var type = typeof(StructConstrainedContainer<>);

            // Act
            var result = type.SatisfiesConstraints(typeof(string));

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies that SatisfiesConstraints returns true when type arguments satisfy multiple constraints.
        /// </summary>
        [Test]
        public void SatisfiesConstraints_MultipleConstraintsWithValidType_ReturnsTrue()
        {
            // Arrange
            var type = typeof(MultiConstrainedContainer<>);

            // Act - List<int> is a class and has a parameterless constructor
            var result = type.SatisfiesConstraints(typeof(List<int>));

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifies that SatisfiesConstraints returns false when type arguments violate one of multiple constraints.
        /// </summary>
        [Test]
        public void SatisfiesConstraints_MultipleConstraintsWithInvalidType_ReturnsFalse()
        {
            // Arrange
            var type = typeof(MultiConstrainedContainer<>);

            // Act - string is a class but doesn't have a parameterless constructor
            var result = type.SatisfiesConstraints(typeof(string));

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies that SatisfiesConstraints handles multiple type arguments correctly.
        /// </summary>
        [Test]
        public void SatisfiesConstraints_MultipleTypeArguments_ReturnsCorrectResult()
        {
            // Arrange
            var type = typeof(DoubleContainer<,>);

            // Act
            var result = type.SatisfiesConstraints(typeof(string), typeof(int));

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region MakeGenericTypeExtended

        /// <summary>
        /// Verifies that MakeGenericTypeExtended creates type from generic type definition.
        /// </summary>
        [Test]
        public void MakeGenericTypeExtended_GenericTypeDefinition_ReturnsConstructedType()
        {
            // Arrange
            var typeDefinition = typeof(SimpleContainer<>);

            // Act
            var result = typeDefinition.MakeGenericTypeExtended(typeof(int));

            // Assert
            Assert.AreEqual(typeof(SimpleContainer<int>), result);
        }

        /// <summary>
        /// Verifies that MakeGenericTypeExtended completes partially constructed type.
        /// </summary>
        [Test]
        public void MakeGenericTypeExtended_PartiallyConstructedType_ReturnsCompletedType()
        {
            // Arrange
            var dictTypeDef = typeof(Dictionary<,>);
            var tValueParam = dictTypeDef.GetGenericArguments()[1];
            var partiallyConstructed = dictTypeDef.MakeGenericType(typeof(string), tValueParam);

            // Act
            var result = partiallyConstructed.MakeGenericTypeExtended(typeof(int));

            // Assert
            Assert.AreEqual(typeof(Dictionary<string, int>), result);
        }

        /// <summary>
        /// Verifies that MakeGenericTypeExtended creates single-dimensional array type.
        /// </summary>
        [Test]
        public void MakeGenericTypeExtended_GenericParameterArray_ReturnsConcreteArrayType()
        {
            // Arrange
            var listTypeDef = typeof(List<>);
            var tParam = listTypeDef.GetGenericArguments()[0];
            var arrayType = tParam.MakeArrayType();

            // Act
            var result = arrayType.MakeGenericTypeExtended(typeof(int));

            // Assert
            Assert.AreEqual(typeof(int[]), result);
        }

        /// <summary>
        /// Verifies that MakeGenericTypeExtended creates two-dimensional array type.
        /// </summary>
        [Test]
        public void MakeGenericTypeExtended_GenericParameterTwoDArray_ReturnsConcreteTwoDArrayType()
        {
            // Arrange
            var listTypeDef = typeof(List<>);
            var tParam = listTypeDef.GetGenericArguments()[0];
            var arrayType = tParam.MakeArrayType(2);

            // Act
            var result = arrayType.MakeGenericTypeExtended(typeof(int));

            // Assert
            Assert.AreEqual(typeof(int[,]), result);
        }

        /// <summary>
        /// Verifies that MakeGenericTypeExtended creates array of generic type.
        /// </summary>
        [Test]
        public void MakeGenericTypeExtended_GenericTypeArray_ReturnsConcreteArrayType()
        {
            // Arrange
            var listTypeDef = typeof(List<>);
            var tParam = listTypeDef.GetGenericArguments()[0];
            var listType = listTypeDef.MakeGenericType(tParam);
            var arrayType = listType.MakeArrayType();

            // Act
            var result = arrayType.MakeGenericTypeExtended(typeof(int));

            // Assert
            Assert.AreEqual(typeof(List<int>[]), result);
        }

        /// <summary>
        /// Verifies that MakeGenericTypeExtended throws ArgumentNullException when type is null.
        /// </summary>
        [Test]
        public void MakeGenericTypeExtended_NullType_ThrowsArgumentNullException()
        {
            // Arrange
            Type nullType = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => nullType.MakeGenericTypeExtended(typeof(int)));
            Assert.AreEqual("openGenericType", ex.ParamName);
        }

        /// <summary>
        /// Verifies that MakeGenericTypeExtended throws ArgumentNullException when arguments are null.
        /// </summary>
        [Test]
        public void MakeGenericTypeExtended_NullArguments_ThrowsArgumentNullException()
        {
            // Arrange
            var typeDefinition = typeof(SimpleContainer<>);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => typeDefinition.MakeGenericTypeExtended(null));
            Assert.AreEqual("providedTypeArguments", ex.ParamName);
        }

        /// <summary>
        /// Verifies that MakeGenericTypeExtended throws ArgumentException for non-generic type.
        /// </summary>
        [Test]
        public void MakeGenericTypeExtended_NonGenericType_ThrowsArgumentException()
        {
            // Arrange
            var nonGenericType = typeof(int);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => nonGenericType.MakeGenericTypeExtended(typeof(string)));
            Assert.That(ex.Message, Does.Contain("must be a generic type"));
        }

        /// <summary>
        /// Verifies that MakeGenericTypeExtended throws ArgumentException for mismatched argument count.
        /// </summary>
        [Test]
        public void MakeGenericTypeExtended_MismatchedArgumentCount_ThrowsArgumentException()
        {
            // Arrange
            var typeDefinition = typeof(DoubleContainer<,>);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => typeDefinition.MakeGenericTypeExtended(typeof(string)));
        }

        #endregion

        #region GetMergedGenericArguments

        /// <summary>
        /// Verifies that GetMergedGenericArguments returns provided arguments for generic type definition.
        /// </summary>
        [Test]
        public void GetMergedGenericArguments_GenericTypeDefinition_ReturnsProvidedArguments()
        {
            // Arrange
            var typeDefinition = typeof(SimpleContainer<>);

            // Act
            var result = typeDefinition.GetMergedGenericArguments(typeof(int));

            // Assert
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(typeof(int), result[0]);
        }

        /// <summary>
        /// Verifies that GetMergedGenericArguments merges existing and provided arguments.
        /// </summary>
        [Test]
        public void GetMergedGenericArguments_PartiallyConstructedType_ReturnsMergedArguments()
        {
            // Arrange
            var dictTypeDef = typeof(Dictionary<,>);
            var tValueParam = dictTypeDef.GetGenericArguments()[1];
            var partiallyConstructed = dictTypeDef.MakeGenericType(typeof(string), tValueParam);

            // Act
            var result = partiallyConstructed.GetMergedGenericArguments(typeof(int));

            // Assert
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(typeof(string), result[0]);
            Assert.AreEqual(typeof(int), result[1]);
        }

        /// <summary>
        /// Verifies that GetMergedGenericArguments preserves existing substituted arguments.
        /// </summary>
        [Test]
        public void GetMergedGenericArguments_WithSubstitutedArguments_PreservesExisting()
        {
            // Arrange
            var dictTypeDef = typeof(Dictionary<,>);
            var tValueParam = dictTypeDef.GetGenericArguments()[1];
            var partiallyConstructed = dictTypeDef.MakeGenericType(typeof(string), tValueParam);

            // Act
            var result = partiallyConstructed.GetMergedGenericArguments(typeof(int));

            // Assert
            Assert.AreEqual(typeof(string), result[0]);
            Assert.AreEqual(typeof(int), result[1]);
        }

        /// <summary>
        /// Verifies that GetMergedGenericArguments throws ArgumentNullException when type is null.
        /// </summary>
        [Test]
        public void GetMergedGenericArguments_NullType_ThrowsArgumentNullException()
        {
            // Arrange
            Type nullType = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => nullType.GetMergedGenericArguments(typeof(int)));
            Assert.AreEqual("openGenericType", ex.ParamName);
        }

        /// <summary>
        /// Verifies that GetMergedGenericArguments throws ArgumentException for non-generic type.
        /// </summary>
        [Test]
        public void GetMergedGenericArguments_NonGenericType_ThrowsArgumentException()
        {
            // Arrange
            var nonGenericType = typeof(int);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => nonGenericType.GetMergedGenericArguments(typeof(string)));
            Assert.That(ex.Message, Does.Contain("must be a generic type"));
        }

        #endregion

        #region GetCompletedGenericArguments

        /// <summary>
        /// Verifies that GetCompletedGenericArguments returns missing arguments for partially constructed type.
        /// </summary>
        [Test]
        public void GetCompletedGenericArguments_PartiallyConstructedType_ReturnsMissingArguments()
        {
            // Arrange
            var dictTypeDef = typeof(Dictionary<,>);
            var tValueParam = dictTypeDef.GetGenericArguments()[1];
            var partiallyConstructed = dictTypeDef.MakeGenericType(typeof(string), tValueParam);
            var targetType = typeof(Dictionary<string, int>);

            // Act
            var result = partiallyConstructed.GetCompletedGenericArguments(targetType);

            // Assert
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(typeof(int), result[0]);
        }

        /// <summary>
        /// Verifies that GetCompletedGenericArguments returns empty array for fully constructed type.
        /// </summary>
        [Test]
        public void GetCompletedGenericArguments_FullyConstructedType_ReturnsEmptyArray()
        {
            // Arrange
            var type = typeof(SimpleContainer<int>);
            var targetType = typeof(SimpleContainer<int>);

            // Act
            var result = type.GetCompletedGenericArguments(targetType);

            // Assert
            Assert.AreEqual(0, result.Length);
        }

        /// <summary>
        /// Verifies that GetCompletedGenericArguments works with allowTypeInheritance.
        /// </summary>
        [Test]
        public void GetCompletedGenericArguments_WithTypeInheritance_ReturnsCorrectArguments()
        {
            // Arrange
            var typeDefinition = typeof(SimpleContainer<>);
            var targetType = typeof(DerivedContainer);

            // Act
            var result = typeDefinition.GetCompletedGenericArguments(targetType, true);

            // Assert
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(typeof(int), result[0]);
        }

        /// <summary>
        /// Verifies that GetCompletedGenericArguments handles array types correctly.
        /// </summary>
        [Test]
        public void GetCompletedGenericArguments_ArrayType_ReturnsCorrectArguments()
        {
            // Arrange
            var listTypeDef = typeof(List<>);
            var tParam = listTypeDef.GetGenericArguments()[0];
            var arrayType = listTypeDef.MakeGenericType(tParam).MakeArrayType();
            var targetType = typeof(List<int>[]);

            // Act
            var result = arrayType.GetCompletedGenericArguments(targetType);

            // Assert
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(typeof(int), result[0]);
        }

        /// <summary>
        /// Verifies that GetCompletedGenericArguments throws ArgumentNullException when type is null.
        /// </summary>
        [Test]
        public void GetCompletedGenericArguments_NullType_ThrowsArgumentNullException()
        {
            // Arrange
            Type nullType = null;
            var targetType = typeof(SimpleContainer<int>);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => nullType.GetCompletedGenericArguments(targetType));
            Assert.AreEqual("openGenericType", ex.ParamName);
        }

        /// <summary>
        /// Verifies that GetCompletedGenericArguments throws when type arguments don't match.
        /// </summary>
        [Test]
        public void GetCompletedGenericArguments_MismatchedArguments_ThrowsArgumentException()
        {
            // Arrange
            var dictTypeDef = typeof(Dictionary<,>);
            var tValueParam = dictTypeDef.GetGenericArguments()[1];
            var partiallyConstructed = dictTypeDef.MakeGenericType(typeof(string), tValueParam);
            var targetType = typeof(Dictionary<int, bool>); // Different key type

            // Act & Assert
            Assert.Throws<ArgumentException>(() => partiallyConstructed.GetCompletedGenericArguments(targetType));
        }

        #endregion
    }
}
