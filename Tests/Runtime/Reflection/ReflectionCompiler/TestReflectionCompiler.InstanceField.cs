using System;
using System.Reflection;
using NUnit.Framework;
using EasyToolkit.Core.Reflection;

namespace EasyToolkit.Core.Reflection.Tests
{
    public class TestReflectionCompiler_InstanceField
    {
        #region Instance Field Tests

        /// <summary>
        /// Verifies that CreateInstanceFieldGetter creates a getter that retrieves the instance field value.
        /// </summary>
        [Test]
        public void CreateInstanceFieldGetter_ValidField_ReturnsFieldValue()
        {
            // Arrange
            var testInstance = new TestClass { InstanceField = 42 };
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.InstanceField),
                MemberAccessFlags.PublicInstance);

            // Act
            var getter = ReflectionCompiler.CreateInstanceFieldGetter(fieldInfo);
            var result = getter(testInstance);

            // Assert
            Assert.AreEqual(42, result);
        }

        /// <summary>
        /// Verifies that CreateInstanceFieldGetter throws ArgumentNullException when fieldInfo is null.
        /// </summary>
        [Test]
        public void CreateInstanceFieldGetter_NullFieldInfo_ThrowsArgumentNullException()
        {
            // Arrange
            FieldInfo fieldInfo = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReflectionCompiler.CreateInstanceFieldGetter(fieldInfo));
        }

        /// <summary>
        /// Verifies that CreateInstanceFieldGetter throws ArgumentException when field is static.
        /// </summary>
        [Test]
        public void CreateInstanceFieldGetter_StaticField_ThrowsArgumentException()
        {
            // Arrange
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.StaticField),
                MemberAccessFlags.PublicStatic);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => ReflectionCompiler.CreateInstanceFieldGetter(fieldInfo));
            Assert.That(ex.Message, Does.Contain("not an instance field"));
        }

        /// <summary>
        /// Verifies that CreateInstanceFieldSetter creates a setter that modifies the instance field value.
        /// </summary>
        [Test]
        public void CreateInstanceFieldSetter_ValidField_SetsFieldValue()
        {
            // Arrange
            var testInstance = new TestClass { InstanceField = 0 };
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.InstanceField),
                MemberAccessFlags.PublicInstance);
            var setter = ReflectionCompiler.CreateInstanceFieldSetter(fieldInfo);

            // Act
            var obj = (object)testInstance;
            setter(ref obj, 100);

            // Assert
            Assert.AreEqual(100, testInstance.InstanceField);
        }

        /// <summary>
        /// Verifies that CreateInstanceFieldSetter throws ArgumentNullException when fieldInfo is null.
        /// </summary>
        [Test]
        public void CreateInstanceFieldSetter_NullFieldInfo_ThrowsArgumentNullException()
        {
            // Arrange
            FieldInfo fieldInfo = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReflectionCompiler.CreateInstanceFieldSetter(fieldInfo));
        }

        /// <summary>
        /// Verifies that CreateInstanceFieldSetter throws ArgumentException when field is static.
        /// </summary>
        [Test]
        public void CreateInstanceFieldSetter_StaticField_ThrowsArgumentException()
        {
            // Arrange
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.StaticField),
                MemberAccessFlags.PublicStatic);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => ReflectionCompiler.CreateInstanceFieldSetter(fieldInfo));
            Assert.That(ex.Message, Does.Contain("not an instance field"));
        }

        /// <summary>
        /// Verifies that CreateInstanceFieldSetter throws ArgumentException when field is read-only.
        /// </summary>
        [Test]
        public void CreateInstanceFieldSetter_ReadOnlyField_ThrowsArgumentException()
        {
            // Arrange
            var fieldInfo = typeof(TestClass).GetField(nameof(TestClass.ReadOnlyField),
                MemberAccessFlags.PublicInstance);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => ReflectionCompiler.CreateInstanceFieldSetter(fieldInfo));
            Assert.That(ex.Message, Does.Contain("read-only"));
        }

        #endregion

        #region Struct Field Tests

        /// <summary>
        /// Verifies that CreateInstanceFieldGetter retrieves the struct field value correctly.
        /// </summary>
        [Test]
        public void CreateInstanceFieldGetter_StructField_ReturnsFieldValue()
        {
            // Arrange
            var testStruct = new TestStruct(42);
            var fieldInfo = typeof(TestStruct).GetField(nameof(TestStruct.Field),
                MemberAccessFlags.PublicInstance);

            // Act
            var getter = ReflectionCompiler.CreateInstanceFieldGetter(fieldInfo);
            var result = getter(testStruct);

            // Assert
            Assert.AreEqual(42, result);
        }

        /// <summary>
        /// Verifies that CreateInstanceFieldSetter modifies the struct field value and preserves the change.
        /// </summary>
        [Test]
        public void CreateInstanceFieldSetter_StructField_SetsFieldValueAndPersists()
        {
            // Arrange
            var testStruct = new TestStruct(10);
            var fieldInfo = typeof(TestStruct).GetField(nameof(TestStruct.Field),
                MemberAccessFlags.PublicInstance);
            var setter = ReflectionCompiler.CreateInstanceFieldSetter(fieldInfo);

            // Act
            var obj = (object)testStruct;
            setter(ref obj, 100);

            // Assert - Verify the modified struct has the new value
            var modifiedStruct = (TestStruct)obj;
            Assert.AreEqual(100, modifiedStruct.Field);
        }

        /// <summary>
        /// Verifies that getter and setter work together for struct fields, confirming value persistence after modification.
        /// </summary>
        [Test]
        public void CreateInstanceFieldGetterSetter_StructField_ValuePersistsAfterModification()
        {
            // Arrange
            var originalStruct = new TestStruct(5);
            var fieldInfo = typeof(TestStruct).GetField(nameof(TestStruct.Field),
                MemberAccessFlags.PublicInstance);
            var getter = ReflectionCompiler.CreateInstanceFieldGetter(fieldInfo);
            var setter = ReflectionCompiler.CreateInstanceFieldSetter(fieldInfo);

            // Act - Get initial value
            var initialValue = getter(originalStruct);
            Assert.AreEqual(5, initialValue, "Initial value should be 5");

            // Act - Modify the struct field
            var obj = (object)originalStruct;
            setter(ref obj, 999);

            // Act - Get modified value
            var modifiedValue = getter(obj);

            // Assert - Verify value persists after modification
            Assert.AreEqual(999, modifiedValue, "Modified value should be 999");
        }

        #endregion
    }
}
