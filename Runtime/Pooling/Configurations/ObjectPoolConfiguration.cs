using System;

namespace EasyToolkit.Core.Pooling
{
    /// <summary>
    /// Configuration for object pools.
    /// Provides properties for configuring object pool behavior.
    /// </summary>
    /// <typeparam name="T">The type of objects managed by the pool.</typeparam>
    public sealed class ObjectPoolConfiguration<T> : PoolConfigurationBase where T : class, new()
    {

        /// <summary>
        /// Gets or sets the allocator function for creating new instances.
        /// If <c>null</c>, uses the default constructor.
        /// </summary>
        public Func<T> Allocator { get; set; }

        /// <summary>
        /// Gets or sets whether to use FastCache as a hot cache layer for frequently accessed objects.
        /// When enabled, the first 4 instances are stored in FastCache for zero-allocation access.
        /// </summary>
        public bool UseFastCache { get; set; } = true;
    }
}
