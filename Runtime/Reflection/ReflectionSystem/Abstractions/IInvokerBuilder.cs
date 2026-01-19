using System;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Builder interface for creating method invokers that call methods with optional parameters.
    /// </summary>
    public interface IInvokerBuilder : IReflectionBuilder
    {
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
