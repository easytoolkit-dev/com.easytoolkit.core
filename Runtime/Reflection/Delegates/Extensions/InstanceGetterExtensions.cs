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
                var result = untypedGetter(obj);
                instance = (TInstance)obj;
                return (TValue)result;
            };
        }
    }
}
