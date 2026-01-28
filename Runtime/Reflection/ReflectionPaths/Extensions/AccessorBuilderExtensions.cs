using System;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Extension methods for building strongly-typed member accessors.
    /// </summary>
    public static class AccessorBuilderExtensions
    {
        /// <summary>
        /// Builds a strongly-typed static getter delegate for a static member.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to retrieve.</typeparam>
        /// <param name="builder">The accessor builder.</param>
        /// <param name="targetType">The type containing the static member.</param>
        /// <returns>A strongly-typed <see cref="StaticGetter{TValue}"/> delegate.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid.</exception>
        public static StaticGetter<TValue> BuildStaticGetter<TValue>(this IAccessorBuilder builder, Type targetType)
        {
            return builder.BuildStaticGetter(targetType).AsTyped<TValue>();
        }

        /// <summary>
        /// Builds a fully-typed instance getter delegate for an instance member.
        /// </summary>
        /// <typeparam name="TInstance">The type of the target instance.</typeparam>
        /// <typeparam name="TValue">The type of the value to retrieve.</typeparam>
        /// <param name="builder">The accessor builder.</param>
        /// <param name="targetType">The type containing the instance member.</param>
        /// <returns>A strongly-typed <see cref="InstanceGetter{TInstance, TValue}"/> delegate.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid.</exception>
        public static InstanceGetter<TInstance, TValue> BuildInstanceGetter<TInstance, TValue>(
            this IAccessorBuilder builder, Type targetType)
        {
            return builder.BuildInstanceGetter(targetType).AsTyped<TInstance, TValue>();
        }

        /// <summary>
        /// Builds a fully-typed instance getter delegate for an instance member.
        /// </summary>
        /// <typeparam name="TInstance">The type of the target instance.</typeparam>
        /// <typeparam name="TValue">The type of the value to retrieve.</typeparam>
        /// <param name="builder">The accessor builder.</param>
        /// <returns>A strongly-typed <see cref="InstanceGetter{TInstance, TValue}"/> delegate.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid.</exception>
        public static InstanceGetter<TInstance, TValue> BuildInstanceGetter<TInstance, TValue>(
            this IAccessorBuilder builder)
        {
            return builder.BuildInstanceGetter(typeof(TInstance)).AsTyped<TInstance, TValue>();
        }

        /// <summary>
        /// Builds a strongly-typed static setter delegate for a static member.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to set.</typeparam>
        /// <param name="builder">The accessor builder.</param>
        /// <param name="targetType">The type containing the static member.</param>
        /// <returns>A strongly-typed <see cref="StaticSetter{TValue}"/> delegate.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid or cannot be set.</exception>
        /// <remarks>
        /// Only fields and properties can be set. The final member in the path must be a field or property.
        /// </remarks>
        public static StaticSetter<TValue> BuildStaticSetter<TValue>(this IAccessorBuilder builder, Type targetType)
        {
            return builder.BuildStaticSetter(targetType).AsTyped<TValue>();
        }

        /// <summary>
        /// Builds a fully-typed instance setter delegate for an instance member.
        /// </summary>
        /// <typeparam name="TInstance">The type of the target instance.</typeparam>
        /// <typeparam name="TValue">The type of the value to assign.</typeparam>
        /// <param name="builder">The accessor builder.</param>
        /// <param name="targetType">The type containing the instance member.</param>
        /// <returns>A strongly-typed <see cref="InstanceSetter{TInstance, TValue}"/> delegate.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid or cannot be set.</exception>
        /// <remarks>
        /// Only fields and properties can be set. The final member in the path must be a field or property.
        /// </remarks>
        public static InstanceSetter<TInstance, TValue> BuildInstanceSetter<TInstance, TValue>(
            this IAccessorBuilder builder, Type targetType)
        {
            return builder.BuildInstanceSetter(targetType).AsTyped<TInstance, TValue>();
        }

        /// <summary>
        /// Builds a fully-typed instance setter delegate for an instance member.
        /// </summary>
        /// <typeparam name="TInstance">The type of the target instance.</typeparam>
        /// <typeparam name="TValue">The type of the value to assign.</typeparam>
        /// <param name="builder">The accessor builder.</param>
        /// <returns>A strongly-typed <see cref="InstanceSetter{TInstance, TValue}"/> delegate.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid or cannot be set.</exception>
        /// <remarks>
        /// Only fields and properties can be set. The final member in the path must be a field or property.
        /// </remarks>
        public static InstanceSetter<TInstance, TValue> BuildInstanceSetter<TInstance, TValue>(
            this IAccessorBuilder builder)
        {
            return builder.BuildInstanceSetter(typeof(TInstance)).AsTyped<TInstance, TValue>();
        }
    }
}
