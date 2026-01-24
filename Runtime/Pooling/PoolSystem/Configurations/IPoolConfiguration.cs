using System;

namespace EasyToolKit.Core.Pooling
{
    /// <summary>
    /// Base configuration interface for pools.
    /// Provides mutable builder properties for pool configuration.
    /// </summary>
    public interface IPoolConfiguration
    {
        /// <summary>
        /// Gets or sets the initial capacity of the pool.
        /// The pool will preallocate this number of instances upon creation.
        /// </summary>
        int InitialCapacity { get; set; }

        /// <summary>
        /// Gets or sets the maximum capacity of the pool.
        /// Values less than zero indicate unlimited capacity.
        /// </summary>
        int MaxCapacity { get; set; }

        /// <summary>
        /// Gets or sets whether to call <see cref="IPoolItem"/> callbacks on pooled objects.
        /// </summary>
        bool CallPoolItemCallbacks { get; set; }

        /// <summary>
        /// Validates the current configuration.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when validation fails.
        /// </exception>
        void Validate();
    }
}
