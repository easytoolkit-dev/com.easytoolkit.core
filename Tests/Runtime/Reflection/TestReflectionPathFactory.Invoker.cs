using System;
using NUnit.Framework;
using EasyToolkit.Core.Reflection;

namespace EasyToolkit.Core.Reflection.Tests
{
    /// <summary>
    /// Tests for the ReflectionPathFactory Invoker functionality.
    /// </summary>
    public class TestReflectionPathFactory_Invoker
    {
        #region BuildStaticFunc Tests - Parameterless

        /// <summary>
        /// Verifies that BuildStaticFunc invokes a static method without parameters.
        /// </summary>
        [Test]
        public void BuildStaticFunc_ParameterlessMethod_ReturnsCorrectResult()
        {
            // Arrange
            TestInvokerClass.StaticMethodCallCount = 0;
            var builder = new InvokerBuilder("StaticMethod");

            // Act
            var invoker = builder.BuildStaticFunc(typeof(TestInvokerClass));
            var result = invoker();

            // Assert
            Assert.AreEqual("StaticResult", result);
            Assert.AreEqual(1, TestInvokerClass.StaticMethodCallCount);
        }

        /// <summary>
        /// Verifies that BuildStaticFunc throws ArgumentException for non-existent method.
        /// </summary>
        [Test]
        public void BuildStaticFunc_NonExistentMethod_ThrowsArgumentException()
        {
            // Arrange
            var builder = new InvokerBuilder("NonExistentMethod");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => builder.BuildStaticFunc(typeof(TestInvokerClass)));
        }

        #endregion

        #region BuildStaticFunc Tests - With Parameters

        /// <summary>
        /// Verifies that BuildStaticFunc invokes a static method with parameters.
        /// </summary>
        [Test]
        public void BuildStaticFunc_MethodWithParams_ReturnsCorrectResult()
        {
            // Arrange
            TestInvokerClass.StaticMethodCallCount = 0;
            var builder = new InvokerBuilder("StaticMethodWithParams");

            // Act
            var invoker = builder.BuildStaticFunc(typeof(TestInvokerClass), typeof(string), typeof(int));
            var result = invoker("Test", 42);

            // Assert
            Assert.AreEqual("Test_42", result);
            Assert.AreEqual(1, TestInvokerClass.StaticMethodCallCount);
        }

        /// <summary>
        /// Verifies that BuildStaticFunc resolves correct overload with no parameters.
        /// </summary>
        [Test]
        public void BuildStaticFunc_OverloadNoParams_CallsCorrectOverload()
        {
            // Arrange
            var builder = new InvokerBuilder("StaticOverloadMethod");

            // Act
            var invoker = builder.BuildStaticFunc(typeof(TestInvokerClass));
            var result = invoker();

            // Assert
            Assert.AreEqual(100, result);
        }

        /// <summary>
        /// Verifies that BuildStaticFunc resolves correct overload with one parameter.
        /// </summary>
        [Test]
        public void BuildStaticFunc_OverloadOneParam_CallsCorrectOverload()
        {
            // Arrange
            var builder = new InvokerBuilder("StaticOverloadMethod");

            // Act
            var invoker = builder.BuildStaticFunc(typeof(TestInvokerClass), typeof(int));
            var result = invoker(15);

            // Assert
            Assert.AreEqual(30, result); // 15 * 2
        }

        /// <summary>
        /// Verifies that BuildStaticFunc resolves correct overload with two parameters.
        /// </summary>
        [Test]
        public void BuildStaticFunc_OverloadTwoParams_CallsCorrectOverload()
        {
            // Arrange
            var builder = new InvokerBuilder("StaticOverloadMethod");

            // Act
            var invoker = builder.BuildStaticFunc(typeof(TestInvokerClass), typeof(int), typeof(int));
            var result = invoker(50, 20);

            // Assert
            Assert.AreEqual(70, result); // 50 + 20
        }

        /// <summary>
        /// Verifies that BuildStaticFunc throws ArgumentException when parameter types don't match.
        /// </summary>
        [Test]
        public void BuildStaticFunc_ParameterTypeMismatch_ThrowsArgumentException()
        {
            // Arrange
            var builder = new InvokerBuilder("StaticMethodWithParams");

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                builder.BuildStaticFunc(typeof(TestInvokerClass), typeof(int), typeof(int)));
        }

        /// <summary>
        /// Verifies that BuildStaticFunc throws ArgumentException when parameter count doesn't match.
        /// </summary>
        [Test]
        public void BuildStaticFunc_ParameterCountMismatch_ThrowsArgumentException()
        {
            // Arrange
            var builder = new InvokerBuilder("StaticMethodWithParams");

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                builder.BuildStaticFunc(typeof(TestInvokerClass), typeof(string)));
        }

        /// <summary>
        /// Verifies that BuildStaticFunc throws ArgumentException when method requires parameters but none provided.
        /// </summary>
        [Test]
        public void BuildStaticFunc_MethodRequiresParams_NoParamsProvided_ThrowsArgumentException()
        {
            // Arrange
            var builder = new InvokerBuilder("StaticMethodWithParams");

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                builder.BuildStaticFunc(typeof(TestInvokerClass)));
        }

        #endregion

        #region BuildStaticFunc Tests - Nested Paths

        /// <summary>
        /// Verifies that BuildStaticFunc can invoke methods through nested static paths.
        /// </summary>
        [Test]
        public void BuildStaticFunc_NestedStaticPath_InvokesNestedMethod()
        {
            // Arrange
            var builder = new InvokerBuilder("StaticNested.NestedMethod");

            // Act
            var invoker = builder.BuildStaticFunc(typeof(TestInvokerClass));
            var result = invoker();

            // Assert
            Assert.AreEqual(3.14159, result);
        }

        /// <summary>
        /// Verifies that BuildStaticFunc can invoke methods with parameters through nested static paths.
        /// </summary>
        [Test]
        public void BuildStaticFunc_NestedStaticPathWithParams_InvokesNestedMethod()
        {
            // Arrange
            var builder = new InvokerBuilder("StaticNested.NestedMethodWithParams");

            // Act
            var invoker = builder.BuildStaticFunc(typeof(TestInvokerClass), typeof(double), typeof(double));
            var result = invoker(5.0, 2.5);

            // Assert
            Assert.AreEqual(12.5, result); // 5.0 * 2.5
        }

        #endregion

        #region BuildInstanceFunc Tests - Parameterless

        /// <summary>
        /// Verifies that BuildInstanceFunc invokes an instance method without parameters.
        /// </summary>
        [Test]
        public void BuildInstanceFunc_ParameterlessMethod_ReturnsCorrectResult()
        {
            // Arrange
            var testInstance = new TestInvokerClass();
            testInstance.InstanceMethodCallCount = 0;
            var builder = new InvokerBuilder("InstanceMethod");

            // Act
            var invoker = builder.BuildInstanceFunc(typeof(TestInvokerClass));
            var obj = (object)testInstance;
            var result = invoker(ref obj);

            // Assert
            Assert.AreEqual("InstanceResult", result);
            Assert.AreEqual(1, testInstance.InstanceMethodCallCount);
        }

        /// <summary>
        /// Verifies that BuildInstanceFunc throws ArgumentException for non-existent method.
        /// </summary>
        [Test]
        public void BuildInstanceFunc_NonExistentMethod_ThrowsArgumentException()
        {
            // Arrange
            var builder = new InvokerBuilder("NonExistentMethod");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => builder.BuildInstanceFunc(typeof(TestInvokerClass)));
        }

        #endregion

        #region BuildInstanceFunc Tests - With Parameters

        /// <summary>
        /// Verifies that BuildInstanceFunc invokes an instance method with parameters.
        /// </summary>
        [Test]
        public void BuildInstanceFunc_MethodWithParams_ReturnsCorrectResult()
        {
            // Arrange
            var testInstance = new TestInvokerClass();
            var builder = new InvokerBuilder("InstanceMethodWithParams");

            // Act
            var invoker = builder.BuildInstanceFunc(typeof(TestInvokerClass), typeof(string), typeof(string));
            var obj = (object)testInstance;
            var result = invoker(ref obj, "Hello", "World");

            // Assert
            Assert.AreEqual("Hello-World", result);
        }

        /// <summary>
        /// Verifies that BuildInstanceFunc resolves correct overload with no parameters.
        /// </summary>
        [Test]
        public void BuildInstanceFunc_OverloadNoParams_CallsCorrectOverload()
        {
            // Arrange
            var testInstance = new TestInvokerClass();
            var builder = new InvokerBuilder("InstanceOverloadMethod");

            // Act
            var invoker = builder.BuildInstanceFunc(typeof(TestInvokerClass));
            var obj = (object)testInstance;
            var result = invoker(ref obj);

            // Assert
            Assert.AreEqual(200, result);
        }

        /// <summary>
        /// Verifies that BuildInstanceFunc resolves correct overload with one parameter.
        /// </summary>
        [Test]
        public void BuildInstanceFunc_OverloadOneParam_CallsCorrectOverload()
        {
            // Arrange
            var testInstance = new TestInvokerClass();
            var builder = new InvokerBuilder("InstanceOverloadMethod");

            // Act
            var invoker = builder.BuildInstanceFunc(typeof(TestInvokerClass), typeof(int));
            var obj = (object)testInstance;
            var result = invoker(ref obj, 10);

            // Assert
            Assert.AreEqual(30, result); // 10 * 3
        }

        /// <summary>
        /// Verifies that BuildInstanceFunc resolves correct overload with two parameters.
        /// </summary>
        [Test]
        public void BuildInstanceFunc_OverloadTwoParams_CallsCorrectOverload()
        {
            // Arrange
            var testInstance = new TestInvokerClass();
            var builder = new InvokerBuilder("InstanceOverloadMethod");

            // Act
            var invoker = builder.BuildInstanceFunc(typeof(TestInvokerClass), typeof(int), typeof(int));
            var obj = (object)testInstance;
            var result = invoker(ref obj, 100, 40);

            // Assert
            Assert.AreEqual(60, result); // 100 - 40
        }

        /// <summary>
        /// Verifies that BuildInstanceFunc throws ArgumentException when parameter types don't match.
        /// </summary>
        [Test]
        public void BuildInstanceFunc_ParameterTypeMismatch_ThrowsArgumentException()
        {
            // Arrange
            var builder = new InvokerBuilder("InstanceMethodWithParams");

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                builder.BuildInstanceFunc(typeof(TestInvokerClass), typeof(int), typeof(int)));
        }

        /// <summary>
        /// Verifies that BuildInstanceFunc throws ArgumentException when parameter count doesn't match.
        /// </summary>
        [Test]
        public void BuildInstanceFunc_ParameterCountMismatch_ThrowsArgumentException()
        {
            // Arrange
            var builder = new InvokerBuilder("InstanceMethodWithParams");

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                builder.BuildInstanceFunc(typeof(TestInvokerClass), typeof(string)));
        }

        /// <summary>
        /// Verifies that BuildInstanceFunc throws ArgumentException when method requires parameters but none provided.
        /// </summary>
        [Test]
        public void BuildInstanceFunc_MethodRequiresParams_NoParamsProvided_ThrowsArgumentException()
        {
            // Arrange
            var builder = new InvokerBuilder("InstanceMethodWithParams");

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                builder.BuildInstanceFunc(typeof(TestInvokerClass)));
        }

        #endregion

        #region BuildInstanceFunc Tests - Nested Paths

        /// <summary>
        /// Verifies that BuildInstanceFunc can invoke methods through nested instance paths.
        /// </summary>
        [Test]
        public void BuildInstanceFunc_NestedInstancePath_InvokesNestedMethod()
        {
            // Arrange
            var testInstance = new TestInvokerClass
            {
                Nested = new NestedInvokerClass()
            };
            var builder = new InvokerBuilder("Nested.NestedMethod");

            // Act
            var invoker = builder.BuildInstanceFunc(typeof(TestInvokerClass));
            var obj = (object)testInstance;
            var result = invoker(ref obj);

            // Assert
            Assert.AreEqual(3.14159, result);
        }

        /// <summary>
        /// Verifies that BuildInstanceFunc can invoke methods with parameters through nested instance paths.
        /// </summary>
        [Test]
        public void BuildInstanceFunc_NestedInstancePathWithParams_InvokesNestedMethod()
        {
            // Arrange
            var testInstance = new TestInvokerClass
            {
                Nested = new NestedInvokerClass()
            };
            var builder = new InvokerBuilder("Nested.NestedMethodWithParams");

            // Act
            var invoker = builder.BuildInstanceFunc(typeof(TestInvokerClass), typeof(double), typeof(double));
            var obj = (object)testInstance;
            var result = invoker(ref obj, 4.0, 3.0);

            // Assert
            Assert.AreEqual(12.0, result); // 4.0 * 3.0
        }

        #endregion

        #region Path Validation Tests

        /// <summary>
        /// Verifies that BuildStaticFunc throws ArgumentException when path ends with non-method member.
        /// </summary>
        [Test]
        public void BuildStaticFunc_PathEndsWithField_ThrowsArgumentException()
        {
            // Arrange
            var builder = new InvokerBuilder("StaticNested.NestedProperty");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => builder.BuildStaticFunc(typeof(TestInvokerClass)));
        }

        /// <summary>
        /// Verifies that BuildInstanceFunc throws ArgumentException when path ends with non-method member.
        /// </summary>
        [Test]
        public void BuildInstanceFunc_PathEndsWithField_ThrowsArgumentException()
        {
            // Arrange
            var builder = new InvokerBuilder("Nested.NestedProperty");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => builder.BuildInstanceFunc(typeof(TestInvokerClass)));
        }

        #endregion
    }
}
