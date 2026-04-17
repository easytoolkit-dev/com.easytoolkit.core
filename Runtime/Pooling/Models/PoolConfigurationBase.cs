using System;

namespace EasyToolkit.Core.Pooling
{
    /// <summary>
    /// Base configuration for object pools.
    /// Provides common properties and validation for pool behavior.
    /// </summary>
    public abstract class PoolConfigurationBase
    {
        protected PoolConfigurationBase(
            int preallocationCount = 0,
            int? maxCapacity = null,
            bool enablePoolItemCallbacks = true)
        {
            if (preallocationCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(preallocationCount), "Preallocation count cannot be negative.");
            }

            if (maxCapacity.HasValue && maxCapacity.Value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxCapacity), "Max capacity cannot be negative.");
            }

            PreallocationCount = preallocationCount;
            MaxCapacity = maxCapacity;
            EnablePoolItemCallbacks = enablePoolItemCallbacks;
        }

        /// <summary>
        /// Gets or sets the number of instances preallocated when the pool is created.
        /// </summary>
        public int PreallocationCount { get; }

        /// <summary>
        /// Gets or sets the maximum capacity of the pool.
        /// A <c>null</c> value indicates unlimited capacity.
        /// </summary>
        public int? MaxCapacity { get; }

        /// <summary>
        /// Gets or sets whether to call <see cref="IPoolObject"/> callbacks on pooled objects.
        /// </summary>
        public bool EnablePoolItemCallbacks { get; }
    }
}
