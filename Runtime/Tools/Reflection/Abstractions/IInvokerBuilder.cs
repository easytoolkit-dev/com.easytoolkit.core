using System;

namespace EasyToolKit.Core
{
    /// <summary>
    /// Builder interface for creating method invokers that call methods with optional parameters.
    /// </summary>
    /// <remarks>
    /// <para>
    /// An invoker creates delegates for calling methods by path or name, with automatic overload resolution
    /// based on parameter types.
    /// </para>
    /// <para>
    /// The method path can be a simple method name or a complex path like "Child.Object.Method()".
    /// The Build methods accept parameter types for overload resolution. The return type is
    /// automatically determined from the method's metadata.
    /// </para>
    /// </remarks>
    public interface IInvokerBuilder
    {
        /// <summary>
        /// Gets the method path this invoker operates on.
        /// </summary>
        string MethodPath { get; }

        /// <summary>
        /// Builds a delegate for invoking a static method with parameters.
        /// </summary>
        /// <param name="targetType">The type containing the static method.</param>
        /// <param name="parameterTypes">The parameter types for overload resolution. Can be null or empty for parameterless methods.</param>
        /// <returns>A delegate that invokes the static method with parameters and returns the result.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        StaticFuncInvoker BuildStaticFunc(Type targetType, params Type[] parameterTypes);

        /// <summary>
        /// Builds a delegate for invoking an instance method with parameters.
        /// </summary>
        /// <param name="targetType">The type containing the instance method.</param>
        /// <param name="parameterTypes">The parameter types for overload resolution. Can be null or empty for parameterless methods.</param>
        /// <returns>A delegate that invokes the instance method with parameters and returns the result.</returns>
        /// <exception cref="ArgumentException">Thrown when the method cannot be found.</exception>
        InstanceFuncInvoker BuildInstanceFunc(Type targetType, params Type[] parameterTypes);
    }
}
