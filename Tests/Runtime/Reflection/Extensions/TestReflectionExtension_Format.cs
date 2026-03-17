using System;
using System.Reflection;
using NUnit.Framework;
using EasyToolkit.Core.Reflection;

namespace EasyToolkit.Core.Reflection.Tests
{
    /// <summary>
    /// Tests for <see cref="ReflectionExtensions.ToCodeString(MemberInfo)"/> and related formatting methods.
    /// </summary>
    public class TestReflectionExtension_Format
    {
        #region Test Helper Classes

        public class TestClassForFormat
        {
            public const int PublicConst = 42;
            public static readonly int PublicStaticReadOnly = 100;
            public static int PublicStaticField;
            public readonly int PublicReadOnlyField = 10;
            public int PublicField;

            private const int PrivateConst = 99;
            private static readonly int PrivateStaticReadOnly = 200;
            private static int PrivateStaticField;
            private readonly int PrivateReadOnlyField = 20;
            private int PrivateField;

            public int PublicProperty { get; set; }
            public int PublicReadOnlyProperty { get; private set; }
            public int PublicWriteOnlyProperty { private get; set; }
            public static int PublicStaticProperty { get; set; }

            public void PublicMethod() { }
            public int PublicMethodWithReturn() => 42;
            public void PublicMethodWithArgs(int a, string b) { }
            public int PublicMethodWithRef(ref int value) => value;
            public void PublicMethodWithOut(out int value) { value = 0; }

            public static void PublicStaticMethod() { }
            public virtual void PublicVirtualMethod() { }

            public TestClassForFormat() { }
            public TestClassForFormat(int value) { }
            protected TestClassForFormat(string value) { }

            public event EventHandler PublicEvent;

            public void GenericMethod<T>() where T : class { }
            public void GenericMethodWithConstraint<T>() where T : new() { }
            public void GenericMethodWithStructConstraint<T>() where T : struct { }
            public void GenericMethodWithTypeConstraint<T>(T value) where T : IDisposable { }
        }

        public abstract class TestAbstractClass
        {
            public abstract void AbstractMethod();
        }

        public class TestEventClass
        {
            public event EventHandler SimpleEvent;
            public event EventHandler<EventArgs> GenericEvent;
        }

        #endregion

        #region FieldInfo - ToCodeString

        /// <summary>
        /// Verifies that ToCodeString formats a public field correctly with default format.
        /// </summary>
        [Test]
        public void ToCodeString_PublicField_ReturnsCorrectFormat()
        {
            // Arrange
            var field = typeof(TestClassForFormat).GetField(nameof(TestClassForFormat.PublicField));

            // Act
            var result = field.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("int"));
            Assert.That(result, Does.Contain(nameof(TestClassForFormat.PublicField)));
            Assert.That(result, Does.EndWith(";"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a static readonly field correctly.
        /// </summary>
        [Test]
        public void ToCodeString_StaticReadOnlyField_ReturnsCorrectFormat()
        {
            // Arrange
            var field = typeof(TestClassForFormat).GetField(nameof(TestClassForFormat.PublicStaticReadOnly));

            // Act
            var result = field.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("static"));
            Assert.That(result, Does.Contain("readonly"));
            Assert.That(result, Does.Contain("int"));
            Assert.That(result, Does.EndWith(";"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a const field with default value when IncludeDefaultValue is specified.
        /// </summary>
        [Test]
        public void ToCodeString_ConstFieldWithDefaultValue_ReturnsCorrectFormat()
        {
            // Arrange
            var field = typeof(TestClassForFormat).GetField(nameof(TestClassForFormat.PublicConst));

            // Act
            var result = field.ToCodeString(MemberFormat.IncludeDefaultValue);

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("const"));
            Assert.That(result, Does.Contain("int"));
            Assert.That(result, Does.Contain("="));
            Assert.That(result, Does.Contain("42"));
        }

        /// <summary>
        /// Verifies that ToCodeString throws ArgumentNullException when field is null.
        /// </summary>
        [Test]
        public void ToCodeString_NullField_ThrowsArgumentNullException()
        {
            // Arrange
            FieldInfo field = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => field.ToCodeString());
        }

        /// <summary>
        /// Verifies that ToCodeString with UseTypeAliases format uses C# type aliases.
        /// </summary>
        [Test]
        public void ToCodeString_WithUseTypeAliases_UsesCSharpAliases()
        {
            // Arrange
            var field = typeof(TestClassForFormat).GetField(nameof(TestClassForFormat.PublicField));

            // Act
            var result = field.ToCodeString(MemberFormat.UseTypeAliases);

            // Assert
            Assert.That(result, Does.Not.Contain("public"));
            Assert.That(result, Does.Contain("int"));
            Assert.That(result, Does.Not.Contain("Int32"));
            Assert.That(result, Does.Contain(nameof(TestClassForFormat.PublicField)));
            Assert.That(result, Does.StartWith("int"));
        }

        #endregion

        #region PropertyInfo - ToCodeString

        /// <summary>
        /// Verifies that ToCodeString formats a public property correctly with default format.
        /// </summary>
        [Test]
        public void ToCodeString_PublicProperty_ReturnsCorrectFormat()
        {
            // Arrange
            var property = typeof(TestClassForFormat).GetProperty(nameof(TestClassForFormat.PublicProperty));

            // Act
            var result = property.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("int"));
            Assert.That(result, Does.Contain(nameof(TestClassForFormat.PublicProperty)));
            Assert.That(result, Does.Contain("{"));
            Assert.That(result, Does.Contain("get;"));
            Assert.That(result, Does.Contain("set;"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a read-only property correctly.
        /// </summary>
        [Test]
        public void ToCodeString_ReadOnlyProperty_ReturnsCorrectFormat()
        {
            // Arrange
            var property = typeof(TestClassForFormat).GetProperty(nameof(TestClassForFormat.PublicReadOnlyProperty));

            // Act
            var result = property.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("get;"));
            Assert.That(result, Does.Contain("private"));
            Assert.That(result, Does.Contain("set;"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a static property correctly.
        /// </summary>
        [Test]
        public void ToCodeString_StaticProperty_ReturnsCorrectFormat()
        {
            // Arrange
            var property = typeof(TestClassForFormat).GetProperty(nameof(TestClassForFormat.PublicStaticProperty));

            // Act
            var result = property.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("static"));
            Assert.That(result, Does.Contain("int"));
        }

        /// <summary>
        /// Verifies that ToCodeString throws ArgumentNullException when property is null.
        /// </summary>
        [Test]
        public void ToCodeString_NullProperty_ThrowsArgumentNullException()
        {
            // Arrange
            PropertyInfo property = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => property.ToCodeString());
        }

        #endregion

        #region MethodInfo - ToCodeString

        /// <summary>
        /// Verifies that ToCodeString formats a public method correctly with default format.
        /// </summary>
        [Test]
        public void ToCodeString_PublicMethod_ReturnsCorrectFormat()
        {
            // Arrange
            var method = typeof(TestClassForFormat).GetMethod(nameof(TestClassForFormat.PublicMethod));

            // Act
            var result = method.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("void"));
            Assert.That(result, Does.Contain(nameof(TestClassForFormat.PublicMethod)));
            Assert.That(result, Does.Contain("("));
            Assert.That(result, Does.Contain(")"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a method with return type correctly.
        /// </summary>
        [Test]
        public void ToCodeString_MethodWithReturnType_ReturnsCorrectFormat()
        {
            // Arrange
            var method = typeof(TestClassForFormat).GetMethod(nameof(TestClassForFormat.PublicMethodWithReturn));

            // Act
            var result = method.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("int"));
            Assert.That(result, Does.Contain(nameof(TestClassForFormat.PublicMethodWithReturn)));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a method with parameters correctly.
        /// </summary>
        [Test]
        public void ToCodeString_MethodWithParameters_ReturnsCorrectFormat()
        {
            // Arrange
            var method = typeof(TestClassForFormat).GetMethod(nameof(TestClassForFormat.PublicMethodWithArgs));

            // Act
            var result = method.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("int"));
            Assert.That(result, Does.Contain("string"));
            Assert.That(result, Does.Contain("a"));
            Assert.That(result, Does.Contain("b"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a static method correctly.
        /// </summary>
        [Test]
        public void ToCodeString_StaticMethod_ReturnsCorrectFormat()
        {
            // Arrange
            var method = typeof(TestClassForFormat).GetMethod(nameof(TestClassForFormat.PublicStaticMethod));

            // Act
            var result = method.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("static"));
            Assert.That(result, Does.Contain("void"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a virtual method correctly.
        /// </summary>
        [Test]
        public void ToCodeString_VirtualMethod_ReturnsCorrectFormat()
        {
            // Arrange
            var method = typeof(TestClassForFormat).GetMethod(nameof(TestClassForFormat.PublicVirtualMethod));

            // Act
            var result = method.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("virtual"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats an abstract method correctly.
        /// </summary>
        [Test]
        public void ToCodeString_AbstractMethod_ReturnsCorrectFormat()
        {
            // Arrange
            var method = typeof(TestAbstractClass).GetMethod(nameof(TestAbstractClass.AbstractMethod));

            // Act
            var result = method.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("abstract"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a generic method correctly.
        /// </summary>
        [Test]
        public void ToCodeString_GenericMethod_ReturnsCorrectFormat()
        {
            // Arrange
            var method = typeof(TestClassForFormat).GetMethod(nameof(TestClassForFormat.GenericMethod));

            // Act
            var result = method.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("void"));
            Assert.That(result, Does.Contain("GenericMethod"));
            Assert.That(result, Does.Contain("<"));
            Assert.That(result, Does.Contain(">"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a generic method with constraints correctly.
        /// </summary>
        [Test]
        public void ToCodeString_GenericMethodWithConstraints_ReturnsCorrectFormat()
        {
            // Arrange
            var method = typeof(TestClassForFormat).GetMethod(nameof(TestClassForFormat.GenericMethodWithConstraint));

            // Act
            var result = method.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("where"));
            Assert.That(result, Does.Contain("T"));
            Assert.That(result, Does.Contain("new()"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a generic method with struct constraint correctly.
        /// </summary>
        [Test]
        public void ToCodeString_GenericMethodWithStructConstraint_ReturnsCorrectFormat()
        {
            // Arrange
            var method = typeof(TestClassForFormat).GetMethod(nameof(TestClassForFormat.GenericMethodWithStructConstraint));

            // Act
            var result = method.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("where"));
            Assert.That(result, Does.Contain("struct"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a generic method with type constraint correctly.
        /// </summary>
        [Test]
        public void ToCodeString_GenericMethodWithTypeConstraint_ReturnsCorrectFormat()
        {
            // Arrange
            var method = typeof(TestClassForFormat).GetMethod(nameof(TestClassForFormat.GenericMethodWithTypeConstraint));

            // Act
            var result = method.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("where"));
            Assert.That(result, Does.Contain("IDisposable"));
        }

        /// <summary>
        /// Verifies that ToCodeString throws ArgumentNullException when method is null.
        /// </summary>
        [Test]
        public void ToCodeString_NullMethod_ThrowsArgumentNullException()
        {
            // Arrange
            MethodInfo method = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => method.ToCodeString());
        }

        #endregion

        #region ConstructorInfo - ToCodeString

        /// <summary>
        /// Verifies that ToCodeString formats a public constructor correctly.
        /// </summary>
        [Test]
        public void ToCodeString_PublicConstructor_ReturnsCorrectFormat()
        {
            // Arrange
            var constructor = typeof(TestClassForFormat).GetConstructor(Array.Empty<Type>());

            // Act
            var result = constructor.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("TestClassForFormat"));
            Assert.That(result, Does.Contain("("));
            Assert.That(result, Does.Contain(")"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a constructor with parameters correctly.
        /// </summary>
        [Test]
        public void ToCodeString_ConstructorWithParameters_ReturnsCorrectFormat()
        {
            // Arrange
            var constructor = typeof(TestClassForFormat).GetConstructor(new[] { typeof(int) });

            // Act
            var result = constructor.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("TestClassForFormat"));
            Assert.That(result, Does.Contain("int"));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a protected constructor correctly.
        /// </summary>
        [Test]
        public void ToCodeString_ProtectedConstructor_ReturnsCorrectFormat()
        {
            // Arrange
            var constructor = typeof(TestClassForFormat).GetConstructor(
                MemberAccessFlags.NonPublicInstance,
                null,
                new[] { typeof(string) },
                null);

            // Act
            var result = constructor.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("protected"));
            Assert.That(result, Does.Contain("TestClassForFormat"));
        }

        /// <summary>
        /// Verifies that ToCodeString throws ArgumentNullException when constructor is null.
        /// </summary>
        [Test]
        public void ToCodeString_NullConstructor_ThrowsArgumentNullException()
        {
            // Arrange
            ConstructorInfo constructor = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => constructor.ToCodeString());
        }

        #endregion

        #region EventInfo - ToCodeString

        /// <summary>
        /// Verifies that ToCodeString formats an event correctly.
        /// </summary>
        [Test]
        public void ToCodeString_Event_ReturnsCorrectFormat()
        {
            // Arrange
            var eventInfo = typeof(TestEventClass).GetEvent(nameof(TestEventClass.SimpleEvent));

            // Act
            var result = eventInfo.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("public"));
            Assert.That(result, Does.Contain("event"));
            Assert.That(result, Does.Contain("EventHandler"));
            Assert.That(result, Does.Contain(nameof(TestEventClass.SimpleEvent)));
        }

        /// <summary>
        /// Verifies that ToCodeString formats a generic event correctly.
        /// </summary>
        [Test]
        public void ToCodeString_GenericEvent_ReturnsCorrectFormat()
        {
            // Arrange
            var eventInfo = typeof(TestEventClass).GetEvent(nameof(TestEventClass.GenericEvent));

            // Act
            var result = eventInfo.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("event"));
            Assert.That(result, Does.Contain("EventHandler"));
        }

        /// <summary>
        /// Verifies that ToCodeString throws ArgumentNullException when event is null.
        /// </summary>
        [Test]
        public void ToCodeString_NullEvent_ThrowsArgumentNullException()
        {
            // Arrange
            EventInfo eventInfo = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => eventInfo.ToCodeString());
        }

        #endregion

        #region MemberInfo - Base ToCodeString (Dispatch Tests)

        /// <summary>
        /// Verifies that ToCodeString correctly dispatches to FieldInfo implementation.
        /// </summary>
        [Test]
        public void ToCodeString_MemberInfoAsField_DispatchesToFieldImplementation()
        {
            // Arrange
            MemberInfo member = typeof(TestClassForFormat).GetField(nameof(TestClassForFormat.PublicField));

            // Act
            var result = member.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("int"));
            Assert.That(result, Does.Contain(nameof(TestClassForFormat.PublicField)));
        }

        /// <summary>
        /// Verifies that ToCodeString correctly dispatches to PropertyInfo implementation.
        /// </summary>
        [Test]
        public void ToCodeString_MemberInfoAsProperty_DispatchesToPropertyImplementation()
        {
            // Arrange
            MemberInfo member = typeof(TestClassForFormat).GetProperty(nameof(TestClassForFormat.PublicProperty));

            // Act
            var result = member.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("int"));
            Assert.That(result, Does.Contain(nameof(TestClassForFormat.PublicProperty)));
            Assert.That(result, Does.Contain("{"));
        }

        /// <summary>
        /// Verifies that ToCodeString correctly dispatches to MethodInfo implementation.
        /// </summary>
        [Test]
        public void ToCodeString_MemberInfoAsMethod_DispatchesToMethodImplementation()
        {
            // Arrange
            MemberInfo member = typeof(TestClassForFormat).GetMethod(nameof(TestClassForFormat.PublicMethod));

            // Act
            var result = member.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("void"));
            Assert.That(result, Does.Contain(nameof(TestClassForFormat.PublicMethod)));
        }

        /// <summary>
        /// Verifies that ToCodeString correctly dispatches to ConstructorInfo implementation.
        /// </summary>
        [Test]
        public void ToCodeString_MemberInfoAsConstructor_DispatchesToConstructorImplementation()
        {
            // Arrange
            MemberInfo member = typeof(TestClassForFormat).GetConstructor(Array.Empty<Type>());

            // Act
            var result = member.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("TestClassForFormat"));
        }

        /// <summary>
        /// Verifies that ToCodeString correctly dispatches to EventInfo implementation.
        /// </summary>
        [Test]
        public void ToCodeString_MemberInfoAsEvent_DispatchesToEventImplementation()
        {
            // Arrange
            MemberInfo member = typeof(TestEventClass).GetEvent(nameof(TestEventClass.SimpleEvent));

            // Act
            var result = member.ToCodeString();

            // Assert
            Assert.That(result, Does.Contain("event"));
        }

        /// <summary>
        /// Verifies that ToCodeString throws NotSupportedException for unsupported member types.
        /// </summary>
        [Test]
        public void ToCodeString_UnsupportedMemberType_ThrowsNotSupportedException()
        {
            // Arrange
            MemberInfo member = typeof(TestClassForFormat); // TypeInfo is not directly supported

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => member.ToCodeString());
        }

        /// <summary>
        /// Verifies that ToCodeString throws ArgumentNullException when member is null.
        /// </summary>
        [Test]
        public void ToCodeString_NullMember_ThrowsArgumentNullException()
        {
            // Arrange
            MemberInfo member = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => member.ToCodeString());
        }

        #endregion

        #region MemberFormat Options

        /// <summary>
        /// Verifies that IncludeModifiers flag includes access modifiers in the output.
        /// </summary>
        [Test]
        public void ToCodeString_WithIncludeModifiers_IncludesModifiers()
        {
            // Arrange
            var field = typeof(TestClassForFormat).GetField(nameof(TestClassForFormat.PublicField));

            // Act
            var result = field.ToCodeString(MemberFormat.IncludeModifiers);

            // Assert
            Assert.That(result, Does.Contain("public"));
        }

        /// <summary>
        /// Verifies that None format excludes modifiers from the output.
        /// </summary>
        [Test]
        public void ToCodeString_WithNoneFormat_ExcludesModifiers()
        {
            // Arrange
            var field = typeof(TestClassForFormat).GetField(nameof(TestClassForFormat.PublicField));

            // Act
            var result = field.ToCodeString(MemberFormat.None);

            // Assert
            Assert.That(result, Does.Not.Contain("public"));
        }

        /// <summary>
        /// Verifies that UseTypeAliases flag uses C# type aliases.
        /// </summary>
        [Test]
        public void ToCodeString_WithUseTypeAliases_UsesTypeAliases()
        {
            // Arrange
            var field = typeof(TestClassForFormat).GetField(nameof(TestClassForFormat.PublicField));

            // Act
            var result = field.ToCodeString(MemberFormat.UseTypeAliases);

            // Assert
            Assert.That(result, Does.Contain("int"));
        }

        #endregion
    }
}
