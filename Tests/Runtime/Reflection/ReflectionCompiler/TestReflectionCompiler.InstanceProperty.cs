using System;
using System.Reflection;
using NUnit.Framework;
using EasyToolkit.Core.Reflection;

namespace EasyToolkit.Core.Reflection.Tests
{
    public class TestReflectionCompiler_InstanceProperty
    {
        #region Instance Property Tests

        /// <summary>
        /// Verifies that CreateInstancePropertyGetter creates a getter that retrieves the instance property value.
        /// </summary>
        [Test]
        public void CreateInstancePropertyGetter_ValidProperty_ReturnsPropertyValue()
        {
            // Arrange
            var testInstance = new TestClass { InstanceProperty = 42 };
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.InstanceProperty),
                MemberAccessFlags.PublicInstance);

            // Act
            var getter = ReflectionCompiler.CreateInstancePropertyGetter(propertyInfo);
            var result = getter(testInstance);

            // Assert
            Assert.AreEqual(42, result);
        }

        /// <summary>
        /// Verifies that CreateInstancePropertyGetter throws ArgumentNullException when propertyInfo is null.
        /// </summary>
        [Test]
        public void CreateInstancePropertyGetter_NullPropertyInfo_ThrowsArgumentNullException()
        {
            // Arrange
            var propertyInfo = null as PropertyInfo;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                ReflectionCompiler.CreateInstancePropertyGetter(propertyInfo));
        }

        /// <summary>
        /// Verifies that CreateInstancePropertyGetter can create a getter for a write-only property with private getter.
        /// </summary>
        [Test]
        public void CreateInstancePropertyGetter_WriteOnlyProperty_ReturnsPropertyValue()
        {
            // Arrange
            var testInstance = new TestClass();
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.WriteOnlyProperty),
                MemberAccessFlags.PublicInstance);
            var getter = ReflectionCompiler.CreateInstancePropertyGetter(propertyInfo);

            // Act - Set value using the property's public setter
            testInstance.WriteOnlyProperty = 42;

            // Assert - Can read value using the compiled getter (accesses private getter)
            var result = getter(testInstance);
            Assert.AreEqual(42, result);
        }

        /// <summary>
        /// Verifies that CreateInstancePropertyGetter throws ArgumentException when property is static.
        /// </summary>
        [Test]
        public void CreateInstancePropertyGetter_StaticProperty_ThrowsArgumentException()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.StaticProperty),
                MemberAccessFlags.PublicStatic);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                ReflectionCompiler.CreateInstancePropertyGetter(propertyInfo));
            Assert.That(ex.Message, Does.Contain("not an instance property"));
        }

        /// <summary>
        /// Verifies that CreateInstancePropertySetter creates a setter that modifies the instance property value.
        /// </summary>
        [Test]
        public void CreateInstancePropertySetter_ValidProperty_SetsPropertyValue()
        {
            // Arrange
            var testInstance = new TestClass { InstanceProperty = 0 };
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.InstanceProperty),
                MemberAccessFlags.PublicInstance);
            var setter = ReflectionCompiler.CreateInstancePropertySetter(propertyInfo);

            // Act
            var obj = (object)testInstance;
            setter(ref obj, 100);

            // Assert
            Assert.AreEqual(100, testInstance.InstanceProperty);
        }

        /// <summary>
        /// Verifies that CreateInstancePropertySetter throws ArgumentNullException when propertyInfo is null.
        /// </summary>
        [Test]
        public void CreateInstancePropertySetter_NullPropertyInfo_ThrowsArgumentNullException()
        {
            // Arrange
            var propertyInfo = null as PropertyInfo;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                ReflectionCompiler.CreateInstancePropertySetter(propertyInfo));
        }

        /// <summary>
        /// Verifies that CreateInstancePropertySetter can create a setter for a read-only property with private setter.
        /// </summary>
        [Test]
        public void CreateInstancePropertySetter_ReadOnlyProperty_SetsPropertyValue()
        {
            // Arrange
            var testInstance = new TestClass();
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.ReadOnlyProperty),
                MemberAccessFlags.PublicInstance);
            var setter = ReflectionCompiler.CreateInstancePropertySetter(propertyInfo);

            // Act - Set value using the compiled setter (accesses private setter)
            var obj = (object)testInstance;
            setter(ref obj, 100);

            // Assert - Can read value using the property's public getter
            Assert.AreEqual(100, testInstance.ReadOnlyProperty);
        }

        /// <summary>
        /// Verifies that CreateInstancePropertySetter throws ArgumentException when property is static.
        /// </summary>
        [Test]
        public void CreateInstancePropertySetter_StaticProperty_ThrowsArgumentException()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.StaticProperty),
                MemberAccessFlags.PublicStatic);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                ReflectionCompiler.CreateInstancePropertySetter(propertyInfo));
            Assert.That(ex.Message, Does.Contain("not an instance property"));
        }

        #endregion

        #region Struct Property Tests

        /// <summary>
        /// Verifies that CreateInstancePropertyGetter retrieves the struct property value correctly.
        /// </summary>
        [Test]
        public void CreateInstancePropertyGetter_StructProperty_ReturnsPropertyValue()
        {
            // Arrange
            var testStruct = new TestStructWithProperty(42);
            var propertyInfo = typeof(TestStructWithProperty).GetProperty(nameof(TestStructWithProperty.Property),
                MemberAccessFlags.PublicInstance);

            // Act
            var getter = ReflectionCompiler.CreateInstancePropertyGetter(propertyInfo);
            var result = getter(testStruct);

            // Assert
            Assert.AreEqual(42, result);
        }

        /// <summary>
        /// Verifies that CreateInstancePropertySetter modifies the struct property value and preserves the change.
        /// </summary>
        [Test]
        public void CreateInstancePropertySetter_StructProperty_SetsPropertyValueAndPersists()
        {
            // Arrange
            var testStruct = new TestStructWithProperty(10);
            var propertyInfo = typeof(TestStructWithProperty).GetProperty(nameof(TestStructWithProperty.Property),
                MemberAccessFlags.PublicInstance);
            var setter = ReflectionCompiler.CreateInstancePropertySetter(propertyInfo);

            // Act
            var obj = (object)testStruct;
            setter(ref obj, 100);

            // Assert - Verify the modified struct has the new value
            var modifiedStruct = (TestStructWithProperty)obj;
            Assert.AreEqual(100, modifiedStruct.Property);
        }

        /// <summary>
        /// Verifies that getter and setter work together for struct properties, confirming value persistence after modification.
        /// </summary>
        [Test]
        public void CreateInstancePropertyGetterSetter_StructProperty_ValuePersistsAfterModification()
        {
            // Arrange
            var originalStruct = new TestStructWithProperty(5);
            var propertyInfo = typeof(TestStructWithProperty).GetProperty(nameof(TestStructWithProperty.Property),
                MemberAccessFlags.PublicInstance);
            var getter = ReflectionCompiler.CreateInstancePropertyGetter(propertyInfo);
            var setter = ReflectionCompiler.CreateInstancePropertySetter(propertyInfo);

            // Act - Get initial value
            var initialValue = getter(originalStruct);
            Assert.AreEqual(5, initialValue, "Initial value should be 5");

            // Act - Modify the struct property
            var obj = (object)originalStruct;
            setter(ref obj, 999);

            // Act - Get modified value
            var modifiedValue = getter(obj);

            // Assert - Verify value persists after modification
            Assert.AreEqual(999, modifiedValue, "Modified value should be 999");
        }

        /// <summary>
        /// Verifies that CreateInstancePropertySetter sets value type property to default value when null is passed.
        /// </summary>
        [Test]
        public void CreateInstancePropertySetter_ValueTypePropertyWithNull_SetsToDefaultValue()
        {
            // Arrange
            var testInstance = new TestClass { InstanceProperty = 42 };
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.InstanceProperty),
                MemberAccessFlags.PublicInstance);
            var setter = ReflectionCompiler.CreateInstancePropertySetter(propertyInfo);

            // Act
            var obj = (object)testInstance;
            setter(ref obj, null);

            // Assert - int default value is 0
            Assert.AreEqual(0, testInstance.InstanceProperty);
        }

        #endregion
    }
}
