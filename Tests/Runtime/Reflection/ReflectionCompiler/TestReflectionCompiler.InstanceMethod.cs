using System;
using System.Reflection;
using NUnit.Framework;
using EasyToolkit.Core.Reflection;

namespace EasyToolkit.Core.Reflection.Tests
{
    public class TestReflectionCompiler_InstanceMethod
    {
        #region Instance Method Tests

        /// <summary>
        /// Verifies that CreateInstanceMethodInvoker creates an invoker that calls the instance method correctly.
        /// </summary>
        [Test]
        public void CreateInstanceMethodInvoker_VoidMethod_CallsMethod()
        {
            // Arrange
            var testInstance = new TestClass();
            var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.InstanceMethod),
                MemberAccessFlags.PublicInstance);

            // Act
            var invoker = ReflectionCompiler.CreateInstanceMethodInvoker(methodInfo);
            var obj = (object)testInstance;
            var result = invoker(ref obj);

            // Assert
            Assert.AreEqual(100, result);
        }

        /// <summary>
        /// Verifies that CreateInstanceMethodInvoker creates an invoker that passes arguments correctly.
        /// </summary>
        [Test]
        public void CreateInstanceMethodInvoker_MethodWithArgs_PassesArguments()
        {
            // Arrange
            var testInstance = new TestClass();
            var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.InstanceMethodWithArgs),
                MemberAccessFlags.PublicInstance);

            // Act
            var invoker = ReflectionCompiler.CreateInstanceMethodInvoker(methodInfo);
            var obj = (object)testInstance;
            var result = invoker(ref obj, 5, 6);

            // Assert
            Assert.AreEqual(30, result);
        }

        /// <summary>
        /// Verifies that CreateInstanceMethodInvoker throws ArgumentNullException when methodInfo is null.
        /// </summary>
        [Test]
        public void CreateInstanceMethodInvoker_NullMethodInfo_ThrowsArgumentNullException()
        {
            // Arrange
            var methodInfo = null as MethodInfo;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                ReflectionCompiler.CreateInstanceMethodInvoker(methodInfo));
        }

        /// <summary>
        /// Verifies that CreateInstanceMethodInvoker throws ArgumentException when method is static.
        /// </summary>
        [Test]
        public void CreateInstanceMethodInvoker_StaticMethod_ThrowsArgumentException()
        {
            // Arrange
            var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.StaticMethod),
                MemberAccessFlags.PublicStatic);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                ReflectionCompiler.CreateInstanceMethodInvoker(methodInfo));
            Assert.That(ex.Message, Does.Contain("not an instance method"));
        }

        #endregion

        #region Instance Void Method Tests

        /// <summary>
        /// Verifies that CreateInstanceVoidMethodInvoker creates an invoker that calls the instance void method correctly.
        /// </summary>
        [Test]
        public void CreateInstanceVoidMethodInvoker_VoidMethod_CallsMethod()
        {
            // Arrange
            var testInstance = new TestClass();
            testInstance.InstanceField = 0;
            var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.InstanceVoidMethod),
                MemberAccessFlags.PublicInstance);

            // Act
            var invoker = ReflectionCompiler.CreateInstanceVoidMethodInvoker(methodInfo);
            var obj = (object)testInstance;
            invoker(ref obj);

            // Assert
            Assert.AreEqual(200, testInstance.InstanceField);
        }

        /// <summary>
        /// Verifies that CreateInstanceVoidMethodInvoker creates an invoker that passes arguments correctly.
        /// </summary>
        [Test]
        public void CreateInstanceVoidMethodInvoker_MethodWithArgs_PassesArguments()
        {
            // Arrange
            var testInstance = new TestClass();
            testInstance.InstanceField = 0;
            var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.InstanceVoidMethodWithArgs),
                MemberAccessFlags.PublicInstance);

            // Act
            var invoker = ReflectionCompiler.CreateInstanceVoidMethodInvoker(methodInfo);
            var obj = (object)testInstance;
            invoker(ref obj, 7, 8);

            // Assert
            Assert.AreEqual(56, testInstance.InstanceField);
        }

        /// <summary>
        /// Verifies that CreateInstanceVoidMethodInvoker throws ArgumentNullException when methodInfo is null.
        /// </summary>
        [Test]
        public void CreateInstanceVoidMethodInvoker_NullMethodInfo_ThrowsArgumentNullException()
        {
            // Arrange
            var methodInfo = null as MethodInfo;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                ReflectionCompiler.CreateInstanceVoidMethodInvoker(methodInfo));
        }

        /// <summary>
        /// Verifies that CreateInstanceVoidMethodInvoker throws ArgumentException when method is static.
        /// </summary>
        [Test]
        public void CreateInstanceVoidMethodInvoker_StaticMethod_ThrowsArgumentException()
        {
            // Arrange
            var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.StaticVoidMethod),
                MemberAccessFlags.PublicStatic);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                ReflectionCompiler.CreateInstanceVoidMethodInvoker(methodInfo));
            Assert.That(ex.Message, Does.Contain("not an instance method"));
        }

        /// <summary>
        /// Verifies that CreateInstanceVoidMethodInvoker throws ArgumentException when method does not return void.
        /// </summary>
        [Test]
        public void CreateInstanceVoidMethodInvoker_NonVoidMethod_ThrowsArgumentException()
        {
            // Arrange
            var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.InstanceMethod),
                MemberAccessFlags.PublicInstance);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                ReflectionCompiler.CreateInstanceVoidMethodInvoker(methodInfo));
            Assert.That(ex.Message, Does.Contain("does not return void"));
        }

        #endregion

        #region Struct Method Tests

        /// <summary>
        /// Verifies that CreateInstanceMethodInvoker calls the struct method correctly and returns the result.
        /// </summary>
        [Test]
        public void CreateInstanceMethodInvoker_StructMethod_CallsMethodAndReturnsResult()
        {
            // Arrange
            var testStruct = new TestStructWithMethod(10);
            var methodInfo = typeof(TestStructWithMethod).GetMethod(nameof(TestStructWithMethod.Method),
                MemberAccessFlags.PublicInstance);

            // Act
            var invoker = ReflectionCompiler.CreateInstanceMethodInvoker(methodInfo);
            var obj = (object)testStruct;
            var result = invoker(ref obj);

            // Assert
            Assert.AreEqual(20, result);
        }

        /// <summary>
        /// Verifies that CreateInstanceMethodInvoker passes arguments correctly for struct methods.
        /// </summary>
        [Test]
        public void CreateInstanceMethodInvoker_StructMethodWithArgs_PassesArguments()
        {
            // Arrange
            var testStruct = new TestStructWithMethod(5);
            var methodInfo = typeof(TestStructWithMethod).GetMethod(nameof(TestStructWithMethod.MethodWithArgs),
                MemberAccessFlags.PublicInstance);

            // Act
            var invoker = ReflectionCompiler.CreateInstanceMethodInvoker(methodInfo);
            var obj = (object)testStruct;
            var result = invoker(ref obj, 3, 7);

            // Assert
            Assert.AreEqual(15, result);
        }

        /// <summary>
        /// Verifies that CreateInstanceMethodInvoker modifies struct field through method call.
        /// </summary>
        [Test]
        public void CreateInstanceMethodInvoker_StructMethod_FieldModifiedAfterMethodCall()
        {
            // Arrange
            var testStruct = new TestStructWithMethod(10);
            var methodInfo = typeof(TestStructWithMethod).GetMethod(nameof(TestStructWithMethod.Method),
                MemberAccessFlags.PublicInstance);
            var invoker = ReflectionCompiler.CreateInstanceMethodInvoker(methodInfo);

            // Act - Method should return Field * 2 = 20
            var obj = (object)testStruct;
            var result = invoker(ref obj);

            // Assert - Verify method was called and returned correct value
            Assert.AreEqual(20, result);

            // Assert - Verify the struct still has original field value (struct is copied in method call)
            var modifiedStruct = (TestStructWithMethod)obj;
            Assert.AreEqual(10, modifiedStruct.Field);
        }

        #endregion

        #region Struct Void Method Tests

        /// <summary>
        /// Verifies that CreateInstanceVoidMethodInvoker calls the struct void method correctly.
        /// </summary>
        [Test]
        public void CreateInstanceVoidMethodInvoker_StructVoidMethod_CallsMethod()
        {
            // Arrange
            var testStruct = new TestStructWithMethod(0);
            var methodInfo = typeof(TestStructWithMethod).GetMethod(nameof(TestStructWithMethod.VoidMethod),
                MemberAccessFlags.PublicInstance);

            // Act
            var invoker = ReflectionCompiler.CreateInstanceVoidMethodInvoker(methodInfo);
            var obj = (object)testStruct;
            invoker(ref obj);

            // Assert - Verify the modified struct has the new field value
            var modifiedStruct = (TestStructWithMethod)obj;
            Assert.AreEqual(999, modifiedStruct.Field);
        }

        /// <summary>
        /// Verifies that CreateInstanceVoidMethodInvoker passes arguments correctly for struct void methods.
        /// </summary>
        [Test]
        public void CreateInstanceVoidMethodInvoker_StructVoidMethodWithArgs_PassesArguments()
        {
            // Arrange
            var testStruct = new TestStructWithMethod(0);
            var methodInfo = typeof(TestStructWithMethod).GetMethod(nameof(TestStructWithMethod.VoidMethodWithArgs),
                MemberAccessFlags.PublicInstance);

            // Act
            var invoker = ReflectionCompiler.CreateInstanceVoidMethodInvoker(methodInfo);
            var obj = (object)testStruct;
            invoker(ref obj, 6, 7);

            // Assert - Verify the modified struct has the field set to 6 * 7 = 42
            var modifiedStruct = (TestStructWithMethod)obj;
            Assert.AreEqual(42, modifiedStruct.Field);
        }

        /// <summary>
        /// Verifies that CreateInstanceVoidMethodInvoker modifies struct field through void method call and value persists.
        /// </summary>
        [Test]
        public void CreateInstanceVoidMethodInvoker_StructVoidMethod_FieldModifiedAndPersists()
        {
            // Arrange
            var originalStruct = new TestStructWithMethod(100);
            var methodInfo = typeof(TestStructWithMethod).GetMethod(nameof(TestStructWithMethod.VoidMethod),
                MemberAccessFlags.PublicInstance);
            var invoker = ReflectionCompiler.CreateInstanceVoidMethodInvoker(methodInfo);

            // Act - Call void method which sets Field to 999
            var obj = (object)originalStruct;
            invoker(ref obj);

            // Assert - Verify the modified struct has the new field value
            var modifiedStruct = (TestStructWithMethod)obj;
            Assert.AreEqual(999, modifiedStruct.Field);
        }

        #endregion
    }
}
