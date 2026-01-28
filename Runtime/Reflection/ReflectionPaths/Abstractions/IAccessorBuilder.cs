using System;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Builder interface for creating member accessors that get or set values through member paths.
    /// </summary>
    public interface IAccessorBuilder : IReflectionBuilder
    {
        /// <summary>
        /// Builds a getter delegate for a static member.
        /// </summary>
        /// <param name="targetType">The type containing the static member.</param>
        /// <returns>A delegate that gets the value from the static member path.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid.</exception>
        StaticGetter BuildStaticGetter(Type targetType);

        /// <summary>
        /// Builds a getter delegate for an instance member.
        /// </summary>
        /// <param name="targetType">The type containing the instance member.</param>
        /// <returns>A delegate that gets the value from the instance member path.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid.</exception>
        InstanceGetter BuildInstanceGetter(Type targetType);

        /// <summary>
        /// Builds a setter delegate for an instance member.
        /// </summary>
        /// <param name="targetType">The type containing the instance member.</param>
        /// <returns>A delegate that sets the value to the instance member path.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid or cannot be set.</exception>
        /// <remarks>
        /// Only fields and properties can be set. The final member in the path must be a field or property.
        /// </remarks>
        InstanceSetter BuildInstanceSetter(Type targetType);

        /// <summary>
        /// Builds a setter delegate for a static member.
        /// </summary>
        /// <param name="targetType">The type containing the static member.</param>
        /// <returns>A delegate that sets the value to the static member path.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid or cannot be set.</exception>
        /// <remarks>
        /// Only fields and properties can be set. The final member in the path must be a field or property.
        /// </remarks>
        StaticSetter BuildStaticSetter(Type targetType);
    }
}
