using System;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Extension methods for converting between generic and non-generic instance setter delegates.
    /// </summary>
    public static class InstanceSetterExtensions
    {
        /// <summary>
        /// Converts a non-generic instance setter to a fully-typed generic version.
        /// </summary>
        /// <typeparam name="TInstance">The type of the target instance.</typeparam>
        /// <typeparam name="TValue">The type of the value to assign.</typeparam>
        /// <param name="untypedSetter">The untyped setter delegate to convert.</param>
        /// <returns>A strongly-typed <see cref="InstanceSetter{TInstance, TValue}"/> delegate.</returns>
        public static InstanceSetter<TInstance, TValue> AsTyped<TInstance, TValue>(
            this InstanceSetter untypedSetter)
        {
            if (untypedSetter == null)
            {
                throw new ArgumentNullException(nameof(untypedSetter));
            }

            return delegate(ref TInstance instance, TValue value)
            {
                object obj = instance;
                untypedSetter(ref obj, value);
                instance = (TInstance)obj;
            };
        }

        /// <summary>
        /// Converts a non-generic instance setter to a partially-typed version with known value type.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to assign.</typeparam>
        /// <param name="untypedSetter">The untyped setter delegate to convert.</param>
        /// <returns>A value-typed <see cref="InstanceSetter{TValue}"/> delegate.</returns>
        public static InstanceSetter<TValue> WithTypedValue<TValue>(
            this InstanceSetter untypedSetter)
        {
            if (untypedSetter == null)
            {
                throw new ArgumentNullException(nameof(untypedSetter));
            }

            return delegate(ref object instance, TValue value) { untypedSetter(ref instance, value); };
        }

        /// <summary>
        /// Converts a fully-typed generic setter to its untyped representation.
        /// </summary>
        /// <typeparam name="TInstance">The type of the target instance.</typeparam>
        /// <typeparam name="TValue">The type of the value to assign.</typeparam>
        /// <param name="typedSetter">The strongly-typed setter delegate to convert.</param>
        /// <returns>An untyped <see cref="InstanceSetter"/> delegate.</returns>
        public static InstanceSetter AsUntyped<TInstance, TValue>(
            this InstanceSetter<TInstance, TValue> typedSetter)
        {
            if (typedSetter == null)
            {
                throw new ArgumentNullException(nameof(typedSetter));
            }

            return delegate(ref object instance, object value)
            {
                var typedInstance = (TInstance)instance;
                typedSetter(ref typedInstance, (TValue)value);
                instance = typedInstance;
            };
        }

        /// <summary>
        /// Converts a partially-typed setter to its untyped representation.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to assign.</typeparam>
        /// <param name="typedSetter">The value-typed setter delegate to convert.</param>
        /// <returns>An untyped <see cref="InstanceSetter"/> delegate.</returns>
        public static InstanceSetter AsUntyped<TValue>(
            this InstanceSetter<TValue> typedSetter)
        {
            if (typedSetter == null)
            {
                throw new ArgumentNullException(nameof(typedSetter));
            }

            return delegate(ref object instance, object value) { typedSetter(ref instance, (TValue)value); };
        }
    }
}
