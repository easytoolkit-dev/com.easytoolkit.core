using System;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Extension methods for converting between generic and non-generic instance getter delegates.
    /// </summary>
    public static class InstanceGetterExtensions
    {
        /// <summary>
        /// Converts a non-generic instance getter to a fully-typed generic version.
        /// </summary>
        /// <typeparam name="TInstance">The type of the target instance.</typeparam>
        /// <typeparam name="TValue">The type of the value to retrieve.</typeparam>
        /// <param name="untypedGetter">The untyped getter delegate to convert.</param>
        /// <returns>A strongly-typed <see cref="InstanceGetter{TInstance, TValue}"/> delegate.</returns>
        public static InstanceGetter<TInstance, TValue> AsTyped<TInstance, TValue>(
            this InstanceGetter untypedGetter)
        {
            if (untypedGetter == null)
            {
                throw new ArgumentNullException(nameof(untypedGetter));
            }

            return delegate(ref TInstance instance)
            {
                object obj = instance;
                var result = untypedGetter(ref obj);
                instance = (TInstance)obj;
                return (TValue)result;
            };
        }

        /// <summary>
        /// Converts a non-generic instance getter to a partially-typed version with known value type.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to retrieve.</typeparam>
        /// <param name="untypedGetter">The untyped getter delegate to convert.</param>
        /// <returns>A value-typed <see cref="InstanceGetter{TValue}"/> delegate.</returns>
        public static InstanceGetter<TValue> WithTypedValue<TValue>(
            this InstanceGetter untypedGetter)
        {
            if (untypedGetter == null)
            {
                throw new ArgumentNullException(nameof(untypedGetter));
            }

            return delegate(ref object instance) { return (TValue)untypedGetter(ref instance); };
        }

        /// <summary>
        /// Converts a fully-typed generic getter to its untyped representation.
        /// </summary>
        /// <typeparam name="TInstance">The type of the target instance.</typeparam>
        /// <typeparam name="TValue">The type of the value to retrieve.</typeparam>
        /// <param name="typedGetter">The strongly-typed getter delegate to convert.</param>
        /// <returns>An untyped <see cref="InstanceGetter"/> delegate.</returns>
        public static InstanceGetter AsUntyped<TInstance, TValue>(
            this InstanceGetter<TInstance, TValue> typedGetter)
        {
            if (typedGetter == null)
            {
                throw new ArgumentNullException(nameof(typedGetter));
            }

            return delegate(ref object instance)
            {
                var typedInstance = (TInstance)instance;
                var result = typedGetter(ref typedInstance);
                instance = typedInstance;
                return result;
            };
        }

        /// <summary>
        /// Converts a partially-typed getter to its untyped representation.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to retrieve.</typeparam>
        /// <param name="typedGetter">The value-typed getter delegate to convert.</param>
        /// <returns>An untyped <see cref="InstanceGetter"/> delegate.</returns>
        public static InstanceGetter AsUntyped<TValue>(
            this InstanceGetter<TValue> typedGetter)
        {
            if (typedGetter == null)
            {
                throw new ArgumentNullException(nameof(typedGetter));
            }

            return delegate(ref object instance) { return typedGetter(ref instance); };
        }
    }
}
