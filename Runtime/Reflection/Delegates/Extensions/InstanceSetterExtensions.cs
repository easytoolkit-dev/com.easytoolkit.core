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
                untypedSetter(obj, value);
                instance = (TInstance)obj;
            };
        }
    }
}
