using System;

namespace EasyToolKit.Core.Pooling.Implementations
{
    /// <summary>
    /// Configuration implementation for object pools.
    /// Provides mutable builder properties and validation for object pools.
    /// </summary>
    /// <typeparam name="T">The type of objects managed by the pool.</typeparam>
    public class ObjectPoolConfiguration<T> : IObjectPoolConfiguration<T> where T : class, new()
    {
        /// <inheritdoc />
        public int InitialCapacity { get; set; }

        /// <inheritdoc />
        public int MaxCapacity { get; set; } = -1;

        /// <inheritdoc />
        public bool CallPoolItemCallbacks { get; set; } = true;

        /// <inheritdoc />
        public Func<T> Allocator { get; set; }

        /// <inheritdoc />
        public bool UseFastCache { get; set; } = true;

        /// <summary>
        /// Validates the current configuration.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when validation fails (e.g., InitialCapacity is negative or exceeds MaxCapacity).
        /// </exception>
        public void Validate()
        {
            if (InitialCapacity < 0)
            {
                throw new InvalidOperationException(
                    $"InitialCapacity cannot be negative. Current value: {InitialCapacity}");
            }

            if (MaxCapacity >= 0 && InitialCapacity > MaxCapacity)
            {
                throw new InvalidOperationException(
                    $"InitialCapacity ({InitialCapacity}) cannot exceed MaxCapacity ({MaxCapacity})");
            }
        }
    }
}
