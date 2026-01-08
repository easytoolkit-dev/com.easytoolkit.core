using System;

namespace EasyToolKit.Core
{
    /// <summary>
    /// Extension methods for <see cref="IAccessorBuilder"/> providing generic API support.
    /// </summary>
    public static class AccessorBuilderExtensions
    {
        /// <summary>
        /// Builds a getter delegate for a static member with a typed return value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to get.</typeparam>
        /// <param name="builder">The accessor builder.</param>
        /// <param name="targetType">The type containing the static member.</param>
        /// <returns>A typed delegate that gets the value from the static member path.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid.</exception>
        public static StaticGetter<TValue> BuildStaticGetter<TValue>(this IAccessorBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            StaticGetter getter = builder.BuildStaticGetter(targetType);
            return () => (TValue)getter();
        }

        /// <summary>
        /// Builds a setter delegate for a static member with a typed value parameter.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to set.</typeparam>
        /// <param name="builder">The accessor builder.</param>
        /// <param name="targetType">The type containing the static member.</param>
        /// <returns>A typed delegate that sets the value to the static member path.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid or cannot be set.</exception>
        /// <remarks>
        /// Only fields and properties can be set. The final member in the path must be a field or property.
        /// </remarks>
        public static StaticSetter<TValue> BuildStaticSetter<TValue>(this IAccessorBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            StaticSetter setter = builder.BuildStaticSetter(targetType);
            return value => setter(value);
        }

        /// <summary>
        /// Builds a getter delegate for an instance member with a typed return value.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TValue">The type of the value to get.</typeparam>
        /// <param name="builder">The accessor builder.</param>
        /// <returns>A typed delegate that gets the value from the instance member path.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid.</exception>
        public static TypedInstanceGetter<TInstance, TValue> BuildTypedInstanceGetter<TInstance, TValue>(this IAccessorBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceGetter getter = builder.BuildInstanceGetter(typeof(TInstance));
            return (ref TInstance instance) =>
            {
                object boxedInstance = instance;
                var result = getter(ref boxedInstance);
                instance = (TInstance)boxedInstance;
                return (TValue)result;
            };
        }

        /// <summary>
        /// Builds a setter delegate for an instance member with typed instance and value parameters.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <typeparam name="TValue">The type of the value to set.</typeparam>
        /// <param name="builder">The accessor builder.</param>
        /// <returns>A typed delegate that sets the value to the instance member path.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid or cannot be set.</exception>
        /// <remarks>
        /// Only fields and properties can be set. The final member in the path must be a field or property.
        /// </remarks>
        public static TypedInstanceSetter<TInstance, TValue> BuildTypedInstanceSetter<TInstance, TValue>(this IAccessorBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceSetter setter = builder.BuildInstanceSetter(typeof(TInstance));
            return (ref TInstance instance, TValue value) =>
            {
                object boxedInstance = instance;
                setter(ref boxedInstance, value);
                instance = (TInstance)boxedInstance;
            };
        }

        /// <summary>
        /// Builds a getter delegate for an instance member with object instance and typed return value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to get.</typeparam>
        /// <param name="builder">The accessor builder.</param>
        /// <param name="targetType">The type containing the instance member.</param>
        /// <returns>A typed delegate that gets the value from the instance member path.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid.</exception>
        public static InstanceGetter<TValue> BuildInstanceGetter<TValue>(this IAccessorBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceGetter getter = builder.BuildInstanceGetter(targetType);
            return (ref object instance) => (TValue)getter(ref instance);
        }

        /// <summary>
        /// Builds a setter delegate for an instance member with object instance and typed value parameter.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to set.</typeparam>
        /// <param name="builder">The accessor builder.</param>
        /// <param name="targetType">The type containing the instance member.</param>
        /// <returns>A typed delegate that sets the value to the instance member path.</returns>
        /// <exception cref="ArgumentException">Thrown when the member path is invalid or cannot be set.</exception>
        /// <remarks>
        /// Only fields and properties can be set. The final member in the path must be a field or property.
        /// </remarks>
        public static InstanceSetter<TValue> BuildInstanceSetter<TValue>(this IAccessorBuilder builder, Type targetType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            InstanceSetter setter = builder.BuildInstanceSetter(targetType);
            return (ref object instance, TValue value) => setter(ref instance, value);
        }
    }
}
