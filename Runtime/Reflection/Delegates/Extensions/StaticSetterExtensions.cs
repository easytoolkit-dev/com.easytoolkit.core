using System;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Extension methods for converting between generic and non-generic static setter delegates.
    /// </summary>
    public static class StaticSetterExtensions
    {
        /// <summary>
        /// Converts a non-generic static setter to a typed generic version.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to set.</typeparam>
        /// <param name="untypedSetter">The untyped setter delegate to convert.</param>
        /// <returns>A strongly-typed <see cref="StaticSetter{TValue}"/> delegate.</returns>
        public static StaticSetter<TValue> AsTyped<TValue>(
            this StaticSetter untypedSetter)
        {
            if (untypedSetter == null)
            {
                throw new ArgumentNullException(nameof(untypedSetter));
            }

            return delegate(TValue value) { untypedSetter(value); };
        }

        /// <summary>
        /// Converts a typed generic setter to its untyped representation.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to set.</typeparam>
        /// <param name="typedSetter">The strongly-typed setter delegate to convert.</param>
        /// <returns>An untyped <see cref="StaticSetter"/> delegate.</returns>
        public static StaticSetter AsUntyped<TValue>(
            this StaticSetter<TValue> typedSetter)
        {
            if (typedSetter == null)
            {
                throw new ArgumentNullException(nameof(typedSetter));
            }

            return delegate(object value) { typedSetter((TValue)value); };
        }
    }
}
