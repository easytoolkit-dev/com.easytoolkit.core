using System;

namespace EasyToolKit.Core.Pooling
{
    /// <summary>
    /// Base configuration for object pools.
    /// Provides common properties and validation for pool behavior.
    /// </summary>
    public abstract class PoolConfigurationBase
    {
        /// <summary>
        /// Gets or sets the initial capacity of the pool.
        /// The pool will preallocate this number of instances upon creation.
        /// </summary>
        public int InitialCapacity { get; set; }

        /// <summary>
        /// Gets or sets the maximum capacity of the pool.
        /// Values less than zero indicate unlimited capacity.
        /// </summary>
        public int MaxCapacity { get; set; } = -1;

        /// <summary>
        /// Gets or sets whether to call <see cref="IPoolItem"/> callbacks on pooled objects.
        /// </summary>
        public bool CallPoolItemCallbacks { get; set; } = true;

        /// <summary>
        /// Validates the current configuration.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when validation fails.
        /// </exception>
        public virtual void Validate()
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
