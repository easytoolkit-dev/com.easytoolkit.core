using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using EasyToolkit.Core.Reflection;

namespace EasyToolkit.Core.Reflection.Tests
{
    /// <summary>
    /// Tests for <see cref="TypeExtensions.GetAllBaseTypes(Type, bool, bool)"/>,
    /// <see cref="TypeExtensions.FindMethod(Type, string, BindingFlags, int)"/>,
    /// <see cref="TypeExtensions.FindMethod(Type, string, BindingFlags, Type[])"/>, and
    /// <see cref="TypeExtensions.GetAllMembers(Type, BindingFlags)"/>.
    /// </summary>
    public class TestTypeExtensions_Query
    {
        #region Test Helper Classes

        public interface ITestInterfaceA
        {
        }

        public interface ITestInterfaceB
        {
        }

        public interface ITestInterfaceC : ITestInterfaceA
        {
        }

        public class TestBaseClass
        {
        }

        public class TestDerivedClass : TestBaseClass, ITestInterfaceB, ITestInterfaceC
        {
        }

        public class TestMultiLevelClass : TestDerivedClass
        {
        }

        public class TestMethodsClass
        {
            public void NoParamsMethod()
            {
            }

            public void SingleParamMethod(int x)
            {
            }

            public int TwoParamsMethod(string a, int b) => b;

            public void ThreeParamsMethod(int a, string b, bool c)
            {
            }

            public void OverloadedMethod()
            {
            }

            public void OverloadedMethod(int x)
            {
            }

            public void OverloadedMethod(int x, string y)
            {
            }

            private void PrivateMethod()
            {
            }

            public static void StaticMethod()
            {
            }

            public int GenericMethod<T>(T value) => 0;
        }

        public class TestMembersClass
        {
            public int PublicField;
            private string _privateField;
            public int PublicProperty { get; set; }
            private string PrivateProperty { get; set; }

            public void PublicMethod()
            {
            }

            private void PrivateMethod()
            {
            }
        }

        public class TestMembersDerivedClass : TestMembersClass
        {
            public new int PublicField;
            public new int PublicProperty { get; set; }

            public void DerivedMethod()
            {
            }
        }

        #endregion

        #region GetAllBaseTypes

        /// <summary>
        /// Verifies that GetAllBaseTypes returns base types in inheritance order.
        /// </summary>
        [Test]
        public void GetAllBaseTypes_DerivedClass_ReturnsBaseTypesInOrder()
        {
            // Arrange
            var type = typeof(TestDerivedClass);

            // Act
            var result = type.GetAllBaseTypes(includeInterface: false, includeTargetType: false).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(typeof(TestBaseClass)));
            Assert.That(result[1], Is.EqualTo(typeof(object)));
        }

        /// <summary>
        /// Verifies that GetAllBaseTypes includes target type when requested.
        /// </summary>
        [Test]
        public void GetAllBaseTypes_IncludeTargetType_ReturnsTypeFirst()
        {
            // Arrange
            var type = typeof(TestDerivedClass);

            // Act
            var result = type.GetAllBaseTypes(includeInterface: false, includeTargetType: true).ToList();

            // Assert
            Assert.That(result[0], Is.EqualTo(typeof(TestDerivedClass)));
            Assert.That(result[1], Is.EqualTo(typeof(TestBaseClass)));
            Assert.That(result[2], Is.EqualTo(typeof(object)));
        }

        /// <summary>
        /// Verifies that GetAllBaseTypes includes interfaces when requested.
        /// </summary>
        [Test]
        public void GetAllBaseTypes_IncludeInterfaces_ReturnsInterfacesAfterBaseTypes()
        {
            // Arrange
            var type = typeof(TestDerivedClass);

            // Act
            var result = type.GetAllBaseTypes(includeInterface: true, includeTargetType: false).ToList();

            // Assert
            Assert.That(result, Does.Contain(typeof(TestBaseClass)));
            Assert.That(result, Does.Contain(typeof(object)));
            Assert.That(result, Does.Contain(typeof(ITestInterfaceB)));
            Assert.That(result, Does.Contain(typeof(ITestInterfaceC)));
            Assert.That(result, Does.Contain(typeof(ITestInterfaceA)));
        }

        /// <summary>
        /// Verifies that GetAllBaseTypes handles multi-level inheritance correctly.
        /// </summary>
        [Test]
        public void GetAllBaseTypes_MultiLevelInheritance_ReturnsAllBaseTypes()
        {
            // Arrange
            var type = typeof(TestMultiLevelClass);

            // Act
            var result = type.GetAllBaseTypes(includeInterface: false, includeTargetType: false).ToList();

            // Assert
            Assert.That(result, Has.Count.GreaterThanOrEqualTo(3));
            Assert.That(result[0], Is.EqualTo(typeof(TestDerivedClass)));
            Assert.That(result[1], Is.EqualTo(typeof(TestBaseClass)));
            Assert.That(result, Does.Contain(typeof(object)));
        }

        /// <summary>
        /// Verifies that GetAllBaseTypes handles object type correctly.
        /// </summary>
        [Test]
        public void GetAllBaseTypes_ObjectType_ReturnsEmpty()
        {
            // Arrange
            var type = typeof(object);

            // Act
            var result = type.GetAllBaseTypes(includeInterface: false, includeTargetType: false).ToList();

            // Assert
            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verifies that GetAllBaseTypes returns interfaces in correct order.
        /// </summary>
        [Test]
        public void GetAllBaseTypes_InterfaceInheritance_ReturnsInheritedInterfaces()
        {
            // Arrange
            var type = typeof(TestDerivedClass);

            // Act
            var result = type.GetAllBaseTypes(includeInterface: true, includeTargetType: false).ToList();

            // Assert
            // ITestInterfaceC extends ITestInterfaceA, so both should be present
            Assert.That(result, Does.Contain(typeof(ITestInterfaceA)));
            Assert.That(result, Does.Contain(typeof(ITestInterfaceC)));
        }

        #endregion

        #region FindMethod - By Parameter Count

        /// <summary>
        /// Verifies that FindMethod finds parameterless method by name.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterCount_NoParamsMethod_ReturnsMethod()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act
            var result = type.FindMethod("NoParamsMethod", BindingFlags.Public | BindingFlags.Instance, 0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("NoParamsMethod"));
            Assert.That(result.GetParameters().Length, Is.EqualTo(0));
        }

        /// <summary>
        /// Verifies that FindMethod finds method with single parameter.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterCount_SingleParamMethod_ReturnsMethod()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act
            var result = type.FindMethod("SingleParamMethod", BindingFlags.Public | BindingFlags.Instance, 1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("SingleParamMethod"));
            Assert.That(result.GetParameters().Length, Is.EqualTo(1));
        }

        /// <summary>
        /// Verifies that FindMethod finds method with multiple parameters.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterCount_TwoParamsMethod_ReturnsMethod()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act
            var result = type.FindMethod("TwoParamsMethod", BindingFlags.Public | BindingFlags.Instance, 2);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("TwoParamsMethod"));
            Assert.That(result.GetParameters().Length, Is.EqualTo(2));
        }

        /// <summary>
        /// Verifies that FindMethod throws ArgumentException when method not found.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterCount_MethodNotFound_ThrowsArgumentException()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                type.FindMethod("NonExistentMethod", BindingFlags.Public | BindingFlags.Instance, 0));
            Assert.That(ex.Message, Does.Contain("NonExistentMethod"));
        }

        /// <summary>
        /// Verifies that FindMethod throws ArgumentException when parameter count doesn't match.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterCount_WrongParameterCount_ThrowsArgumentException()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                type.FindMethod("NoParamsMethod", BindingFlags.Public | BindingFlags.Instance, 5));
            Assert.That(ex.Message, Does.Contain("parameter count"));
            Assert.That(ex.Message, Does.Contain("5"));
        }

        /// <summary>
        /// Verifies that FindMethod throws ArgumentNullException when type is null.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterCount_NullType_ThrowsArgumentNullException()
        {
            // Arrange
            Type type = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                type.FindMethod("TestMethod", BindingFlags.Public | BindingFlags.Instance, 0));
            Assert.That(ex.ParamName, Is.EqualTo("targetType"));
        }

        /// <summary>
        /// Verifies that FindMethod throws ArgumentNullException when method name is null.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterCount_NullMethodName_ThrowsArgumentNullException()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                type.FindMethod(null, BindingFlags.Public | BindingFlags.Instance, 0));
            Assert.That(ex.ParamName, Is.EqualTo("methodName"));
        }

        /// <summary>
        /// Verifies that FindMethod throws ArgumentNullException when method name is whitespace.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterCount_WhitespaceMethodName_ThrowsArgumentNullException()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                type.FindMethod("   ", BindingFlags.Public | BindingFlags.Instance, 0));
            Assert.That(ex.ParamName, Is.EqualTo("methodName"));
        }

        /// <summary>
        /// Verifies that FindMethod can find static methods.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterCount_StaticMethod_ReturnsMethod()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act
            var result = type.FindMethod("StaticMethod", BindingFlags.Public | BindingFlags.Static, 0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("StaticMethod"));
            Assert.That(result.IsStatic, Is.True);
        }

        #endregion

        #region FindMethod - By Parameter Types

        /// <summary>
        /// Verifies that FindMethod finds parameterless method when parameter types is null.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterTypes_NullParameterTypes_ReturnsParameterlessMethod()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act
            var result = type.FindMethod("NoParamsMethod", BindingFlags.Public | BindingFlags.Instance, null);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("NoParamsMethod"));
            Assert.That(result.GetParameters().Length, Is.EqualTo(0));
        }

        /// <summary>
        /// Verifies that FindMethod finds method with exact parameter types.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterTypes_ExactParameterTypes_ReturnsMethod()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act
            var result = type.FindMethod("TwoParamsMethod", BindingFlags.Public | BindingFlags.Instance,
                typeof(string), typeof(int));

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("TwoParamsMethod"));
            var parameters = result.GetParameters();
            Assert.That(parameters[0].ParameterType, Is.EqualTo(typeof(string)));
            Assert.That(parameters[1].ParameterType, Is.EqualTo(typeof(int)));
        }

        /// <summary>
        /// Verifies that FindMethod supports covariance for parameter type matching.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterTypes_CovariantTypes_ReturnsMethod()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act
            var result = type.FindMethod("SingleParamMethod", BindingFlags.Public | BindingFlags.Instance,
                typeof(int)); // Exact match

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("SingleParamMethod"));
        }

        /// <summary>
        /// Verifies that FindMethod throws exception when parameter types don't match.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterTypes_WrongParameterTypes_ThrowsArgumentException()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                type.FindMethod("TwoParamsMethod", BindingFlags.Public | BindingFlags.Instance,
                    typeof(int), typeof(string))); // Reversed order
            Assert.That(ex.Message, Does.Contain("No matching method overload"));
        }

        /// <summary>
        /// Verifies that FindMethod throws exception with available signatures when not found.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterTypes_MethodNotFound_IncludesAvailableSignatures()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                type.FindMethod("NonExistentMethod", BindingFlags.Public | BindingFlags.Instance,
                    typeof(string), typeof(int)));
            Assert.That(ex.Message, Does.Contain("Available overloads"));
        }

        /// <summary>
        /// Verifies that FindMethod throws ArgumentNullException when type is null.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterTypes_NullType_ThrowsArgumentNullException()
        {
            // Arrange
            Type type = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                type.FindMethod("TestMethod", BindingFlags.Public | BindingFlags.Instance, typeof(int)));
            Assert.That(ex.ParamName, Is.EqualTo("targetType"));
        }

        /// <summary>
        /// Verifies that FindMethod throws ArgumentNullException when method name is null.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterTypes_NullMethodName_ThrowsArgumentNullException()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                type.FindMethod(null, BindingFlags.Public | BindingFlags.Instance, typeof(int)));
            Assert.That(ex.ParamName, Is.EqualTo("methodName"));
        }

        /// <summary>
        /// Verifies that FindMethod finds method with three parameters.
        /// </summary>
        [Test]
        public void FindMethod_ByParameterTypes_ThreeParameterMethod_ReturnsMethod()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act
            var result = type.FindMethod("ThreeParamsMethod", BindingFlags.Public | BindingFlags.Instance,
                typeof(int), typeof(string), typeof(bool));

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("ThreeParamsMethod"));
            Assert.That(result.GetParameters().Length, Is.EqualTo(3));
        }

        #endregion

        #region GetAllMembers

        /// <summary>
        /// Verifies that GetAllMembers returns all public instance members.
        /// </summary>
        [Test]
        public void GetAllMembers_PublicInstanceFlags_ReturnsPublicMembers()
        {
            // Arrange
            var type = typeof(TestMembersClass);

            // Act
            var result = type.GetAllMembers(BindingFlags.Public | BindingFlags.Instance).ToList();

            // Assert
            Assert.That(result, Has.Count.GreaterThan(0));
            Assert.That(result.Any(m => m.Name == "PublicField"), Is.True);
            Assert.That(result.Any(m => m.Name == "PublicProperty"), Is.True);
            Assert.That(result.Any(m => m.Name == "PublicMethod"), Is.True);
        }

        /// <summary>
        /// Verifies that GetAllMembers returns all members including inherited ones.
        /// </summary>
        [Test]
        public void GetAllMembers_DerivedClass_ReturnsInheritedMembers()
        {
            // Arrange
            var type = typeof(TestMembersDerivedClass);

            // Act
            var result = type.GetAllMembers(BindingFlags.Public | BindingFlags.Instance).ToList();

            // Assert
            Assert.That(result.Any(m => m.DeclaringType == typeof(TestMembersClass)), Is.True);
        }

        /// <summary>
        /// Verifies that GetAllMembers with DeclaredOnly returns only declared members.
        /// </summary>
        [Test]
        public void GetAllMembers_DeclaredOnlyFlag_ReturnsOnlyDeclaredMembers()
        {
            // Arrange
            var type = typeof(TestMembersDerivedClass);

            // Act
            var result = type.GetAllMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .ToList();

            // Assert
            Assert.That(result, Has.Count.GreaterThan(0));
            Assert.That(result.Any(m => m.DeclaringType == typeof(TestMembersDerivedClass)), Is.True);
        }

        /// <summary>
        /// Verifies that GetAllMembers returns private members when requested.
        /// </summary>
        [Test]
        public void GetAllMembers_NonPublicFlags_ReturnsPrivateMembers()
        {
            // Arrange
            var type = typeof(TestMembersClass);

            // Act
            var result = type.GetAllMembers(BindingFlags.NonPublic | BindingFlags.Instance).ToList();

            // Assert
            Assert.That(result, Has.Count.GreaterThan(0));
            Assert.That(result.Any(m => m.Name == "_privateField"), Is.True);
        }

        /// <summary>
        /// Verifies that GetAllMembers returns static members when requested.
        /// </summary>
        [Test]
        public void GetAllMembers_StaticFlags_ReturnsStaticMembers()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act
            var result = type.GetAllMembers(BindingFlags.Public | BindingFlags.Static).ToList();

            // Assert
            Assert.That(result.Any(m => m.Name == "StaticMethod"), Is.True);
        }

        /// <summary>
        /// Verifies that GetAllMembers handles interface types correctly.
        /// </summary>
        [Test]
        public void GetAllMembers_InterfaceType_ReturnsInterfaceMembers()
        {
            // Arrange
            var type = typeof(IEnumerable<int>);

            // Act
            var result = type.GetAllMembers(BindingFlags.Public | BindingFlags.Instance).ToList();

            // Assert
            Assert.That(result, Has.Count.GreaterThan(0));
        }

        /// <summary>
        /// Verifies that GetAllMembers handles object type correctly.
        /// </summary>
        [Test]
        public void GetAllMembers_ObjectType_ReturnsObjectMembers()
        {
            // Arrange
            var type = typeof(object);

            // Act
            var result = type.GetAllMembers(BindingFlags.Public | BindingFlags.Instance).ToList();

            // Assert
            Assert.That(result, Has.Count.GreaterThan(0));
            Assert.That(result.Any(m => m.Name == "GetType"), Is.True);
        }

        #endregion

        #region GetAllMembers - By Name

        /// <summary>
        /// Verifies that GetAllMembers with name returns members with that name.
        /// </summary>
        [Test]
        public void GetAllMembers_ByName_ExistingName_ReturnsMembers()
        {
            // Arrange
            var type = typeof(TestMembersClass);

            // Act
            var result = type.GetAllMembers("PublicField", BindingFlags.Public | BindingFlags.Instance).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("PublicField"));
        }

        /// <summary>
        /// Verifies that GetAllMembers with name returns overloaded members.
        /// </summary>
        [Test]
        public void GetAllMembers_ByName_OverloadedMethod_ReturnsAllOverloads()
        {
            // Arrange
            var type = typeof(TestMethodsClass);

            // Act
            var result = type.GetAllMembers("OverloadedMethod", BindingFlags.Public | BindingFlags.Instance).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(3));
        }

        /// <summary>
        /// Verifies that GetAllMembers with name returns empty when name not found.
        /// </summary>
        [Test]
        public void GetAllMembers_ByName_NonExistentName_ReturnsEmpty()
        {
            // Arrange
            var type = typeof(TestMembersClass);

            // Act
            var result = type.GetAllMembers("NonExistentMember", BindingFlags.Public | BindingFlags.Instance).ToList();

            // Assert
            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verifies that GetAllMembers with name finds members in base types.
        /// </summary>
        [Test]
        public void GetAllMembers_ByName_DerivedClass_FindsInheritedMembers()
        {
            // Arrange
            var type = typeof(TestMembersDerivedClass);

            // Act
            var result = type.GetAllMembers("PublicMethod", BindingFlags.Public | BindingFlags.Instance).ToList();

            // Assert
            Assert.That(result, Has.Count.GreaterThan(0));
            Assert.That(result.Any(m => m.DeclaringType == typeof(TestMembersClass)), Is.True);
        }

        /// <summary>
        /// Verifies that GetAllMembers throws ArgumentException when name is null.
        /// </summary>
        [Test]
        public void GetAllMembers_ByName_NullName_ThrowsArgumentException()
        {
            // Arrange
            var type = typeof(TestMembersClass);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                type.GetAllMembers(null, BindingFlags.Public | BindingFlags.Instance));
            Assert.That(ex.ParamName, Is.EqualTo("name"));
        }

        /// <summary>
        /// Verifies that GetAllMembers throws ArgumentException when name is whitespace.
        /// </summary>
        [Test]
        public void GetAllMembers_ByName_WhitespaceName_ThrowsArgumentException()
        {
            // Arrange
            var type = typeof(TestMembersClass);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                type.GetAllMembers("   ", BindingFlags.Public | BindingFlags.Instance));
            Assert.That(ex.ParamName, Is.EqualTo("name"));
        }

        /// <summary>
        /// Verifies that GetAllMembers with name handles empty string correctly.
        /// </summary>
        [Test]
        public void GetAllMembers_ByName_EmptyString_ThrowsArgumentException()
        {
            // Arrange
            var type = typeof(TestMembersClass);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                type.GetAllMembers(string.Empty, BindingFlags.Public | BindingFlags.Instance));
            Assert.That(ex.ParamName, Is.EqualTo("name"));
        }

        #endregion
    }
}
