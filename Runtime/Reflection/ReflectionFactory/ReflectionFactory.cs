using System;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Factory for creating reflection-based accessors and invokers with a weak-typed API.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This factory provides a simple, weak-typed API for runtime reflection operations.
    /// It uses object and Type parameters instead of generics, making it ideal for scenarios
    /// where types are determined at runtime, such as in scene managers, editor tools, and
    /// serialization systems.
    /// </para>
    /// <para>
    /// The factory is divided into two main concepts:
    /// <list type="bullet">
    /// <item><description><b>Accessor</b>: For getting and setting member values through paths (fields, properties, parameterless methods)</description></item>
    /// <item><description><b>Invoker</b>: For calling methods with parameters, supports expression paths</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Usage examples:
    /// <code>
    /// // Accessor - Getting a value
    /// IAccessorBuilder accessor = ReflectionFactory.CreateAccessor("Player.Stats.Health");
    /// InstanceGetter getter = accessor.BuildInstanceGetter(typeof(GameManager));
    /// object health = getter(ref gameManager);
    ///
    /// // Accessor - Setting a value
    /// InstanceSetter setter = accessor.BuildInstanceSetter(typeof(GameManager));
    /// setter(ref gameManager, 100);
    ///
    /// // Invoker - Calling a simple method without parameters
    /// IInvokerBuilder invoker = ReflectionFactory.CreateInvoker("CalculateDamage");
    /// InstanceFuncInvoker noParamFunc = invoker.BuildInstanceFunc(typeof(Player));
    /// object result = noParamFunc(ref player, null);
    ///
    /// // Invoker - Calling a method with parameters
    /// InstanceFuncInvoker paramFunc = invoker.BuildInstanceFunc(
    ///     typeof(Player),
    ///     typeof(int),
    ///     typeof(float)
    /// );
    /// object damage = paramFunc(ref player, new object[] { 10, 1.5f });
    ///
    /// // Invoker - Using expression path (e.g., nested object method)
    /// IInvokerBuilder pathInvoker = ReflectionFactory.CreateInvoker("Transform.GetChild");
    /// InstanceFuncInvoker pathFunc = pathInvoker.BuildInstanceFunc(typeof(GameObject), typeof(int));
    /// Transform child = (Transform)pathFunc(ref gameObject, new object[] { 0 });
    ///
    /// // Static members
    /// IAccessorBuilder staticAccessor = ReflectionFactory.CreateAccessor("MaxLevel");
    /// StaticGetter staticGetter = staticAccessor.BuildStaticGetter(typeof(Config));
    /// StaticSetter staticSetter = staticAccessor.BuildStaticSetter(typeof(Config));
    /// </code>
    /// </para>
    /// </remarks>
    public static class ReflectionFactory
    {
        /// <summary>
        /// Creates an accessor builder for getting and setting member values.
        /// </summary>
        /// <param name="memberPath">The path to the member (e.g., "Field", "Property", "Nested.Field", "Player.Stats.Health").</param>
        /// <returns>An accessor builder for configuring and building getter/setter delegates.</returns>
        /// <exception cref="ArgumentException">Thrown when memberPath is null or whitespace.</exception>
        /// <remarks>
        /// <para>
        /// The accessor can traverse complex member paths including:
        /// <list type="bullet">
        /// <item><description>Fields (public and non-public)</description></item>
        /// <item><description>Properties (public and non-public)</description></item>
        /// <item><description>Parameterless methods (invoked as part of the path)</description></item>
        /// <item><description>Array and list elements (e.g., "Items[0]")</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// Note: Only fields and properties at the end of the path can be set.
        /// Methods, array elements, and list elements cannot be set.
        /// </para>
        /// </remarks>
        public static IAccessorBuilder CreateAccessor(string memberPath)
        {
            if (string.IsNullOrWhiteSpace(memberPath))
                throw new ArgumentException("Member path cannot be null or whitespace.", nameof(memberPath));

            return new AccessorBuilder(memberPath);
        }

        /// <summary>
        /// Creates an invoker builder for calling methods with optional parameters.
        /// </summary>
        /// <param name="methodPath">The path to the method (e.g., "Method", "Child.Object.Method()").</param>
        /// <returns>An invoker builder for configuring and building method invocation delegates.</returns>
        /// <exception cref="ArgumentException">Thrown when methodPath is null or whitespace.</exception>
        /// <remarks>
        /// <para>
        /// The invoker supports:
        /// <list type="bullet">
        /// <item><description>Simple method names (e.g., "MethodName")</description></item>
        /// <item><description>Expression paths (e.g., "Child.Object.Method()")</description></item>
        /// <item><description>Automatic overload resolution based on parameter types</description></item>
        /// <item><description>Both instance and static methods</description></item>
        /// <item><description>Methods with 0 to N parameters</description></item>
        /// <item><description>Automatic return type detection from method metadata</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// When using expression paths, the path can traverse through fields, properties, and parameterless methods
        /// to reach the target method. The final element in the path must be a method call.
        /// </para>
        /// </remarks>
        public static IInvokerBuilder CreateInvoker(string methodPath)
        {
            if (string.IsNullOrWhiteSpace(methodPath))
                throw new ArgumentException("Method path cannot be null or whitespace.", nameof(methodPath));

            return new InvokerBuilder(methodPath);
        }
    }
}
