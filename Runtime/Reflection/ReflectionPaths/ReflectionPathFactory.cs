using System;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Factory for creating reflection-based accessors and invokers with a weak-typed API.
    /// </summary>
    public static class ReflectionPathFactory
    {
        /// <summary>
        /// Creates an accessor builder for getting and setting member values.
        /// </summary>
        /// <param name="memberPath">The path to the member (e.g., "Field", "Property", "Nested.Field", "Player.Stats.Health").</param>
        /// <returns>An accessor builder for configuring and building getter/setter delegates.</returns>
        public static IAccessorBuilder BuildAccessor(string memberPath)
        {
            if (string.IsNullOrWhiteSpace(memberPath))
                throw new ArgumentException("Member path cannot be null or whitespace.", nameof(memberPath));

            return new AccessorBuilder(memberPath);
        }

        /// <summary>
        /// Creates an invoker builder for calling methods with optional parameters.
        /// </summary>
        /// <param name="methodPath">The path to the method (e.g., "Method", "Child.Object.Method").</param>
        /// <returns>An invoker builder for configuring and building method invocation delegates.</returns>
        public static IInvokerBuilder BuildInvoker(string methodPath)
        {
            if (string.IsNullOrWhiteSpace(methodPath))
                throw new ArgumentException("Method path cannot be null or whitespace.", nameof(methodPath));

            return new InvokerBuilder(methodPath);
        }
    }
}
