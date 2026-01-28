using System;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Extension methods for converting between generic and non-generic static getter delegates.
    /// </summary>
    public static class StaticGetterExtensions
    {
        /// <summary>
        /// Converts a non-generic static getter to a typed generic version.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to retrieve.</typeparam>
        /// <param name="untypedGetter">The untyped getter delegate to convert.</param>
        /// <returns>A strongly-typed <see cref="StaticGetter{TValue}"/> delegate.</returns>
        public static StaticGetter<TValue> AsTyped<TValue>(
            this StaticGetter untypedGetter)
        {
            if (untypedGetter == null)
            {
                throw new ArgumentNullException(nameof(untypedGetter));
            }

            return delegate { return (TValue)untypedGetter(); };
        }

        /// <summary>
        /// Converts a typed generic getter to its untyped representation.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to retrieve.</typeparam>
        /// <param name="typedGetter">The strongly-typed getter delegate to convert.</param>
        /// <returns>An untyped <see cref="StaticGetter"/> delegate.</returns>
        public static StaticGetter AsUntyped<TValue>(
            this StaticGetter<TValue> typedGetter)
        {
            if (typedGetter == null)
            {
                throw new ArgumentNullException(nameof(typedGetter));
            }

            return delegate { return typedGetter(); };
        }
    }
}
