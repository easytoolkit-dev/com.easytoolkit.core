using System;

namespace EasyToolKit.Core.Pooling
{
    /// <summary>
    /// Configuration interface for C# object pools.
    /// Provides mutable builder properties for object pool configuration.
    /// </summary>
    /// <typeparam name="T">The type of objects managed by the pool.</typeparam>
    public interface IObjectPoolConfiguration<T> : IPoolConfiguration where T : class, new()
    {
        /// <summary>
        /// Gets or sets the allocator function for creating new instances.
        /// If <c>null</c>, uses the default constructor.
        /// </summary>
        Func<T> Allocator { get; set; }

        /// <summary>
        /// Gets or sets whether to use FastCache as a hot cache layer for frequently accessed objects.
        /// When enabled, the first 4 instances are stored in FastCache for zero-allocation access.
        /// </summary>
        bool UseFastCache { get; set; }
    }
}
