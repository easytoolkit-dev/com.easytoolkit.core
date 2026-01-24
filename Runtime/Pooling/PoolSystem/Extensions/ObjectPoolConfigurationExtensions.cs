using System;

namespace EasyToolKit.Core.Pooling
{
    /// <summary>
    /// Extension methods for <see cref="IObjectPoolConfiguration{T}"/> interfaces.
    /// Provides fluent API methods for configuring object pool properties.
    /// </summary>
    public static class ObjectPoolConfigurationExtensions
    {
        /// <summary>
        /// Sets the allocator function for creating new instances.
        /// </summary>
        /// <typeparam name="T">The type of objects managed by the pool.</typeparam>
        /// <typeparam name="TConfiguration">The object pool configuration type.</typeparam>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="allocator">The allocator function to use.</param>
        /// <returns>The configuration instance for method chaining.</returns>
        public static TConfiguration WithAllocator<T, TConfiguration>(
            this TConfiguration configuration, Func<T> allocator)
            where TConfiguration : IObjectPoolConfiguration<T>
            where T : class, new()
        {
            configuration.Allocator = allocator;
            return configuration;
        }

        /// <summary>
        /// Configures the pool to use default constructor for creating instances.
        /// </summary>
        /// <typeparam name="T">The type of objects managed by the pool.</typeparam>
        /// <typeparam name="TConfiguration">The object pool configuration type.</typeparam>
        /// <param name="configuration">The configuration instance.</param>
        /// <returns>The configuration instance for method chaining.</returns>
        public static TConfiguration WithDefaultConstructor<T, TConfiguration>(
            this TConfiguration configuration)
            where TConfiguration : IObjectPoolConfiguration<T>
            where T : class, new()
        {
            configuration.Allocator = null;
            return configuration;
        }

        /// <summary>
        /// Configures the pool to use FastCache as a hot cache layer for frequently accessed objects.
        /// When enabled, the first 4 instances are stored in FastCache for zero-allocation access.
        /// </summary>
        /// <typeparam name="T">The type of objects managed by the pool.</typeparam>
        /// <typeparam name="TConfiguration">The object pool configuration type.</typeparam>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="enabled">Whether to enable FastCache. Default is <c>true</c>.</param>
        /// <returns>The configuration instance for method chaining.</returns>
        public static TConfiguration WithFastCache<T, TConfiguration>(
            this TConfiguration configuration, bool enabled = true)
            where TConfiguration : IObjectPoolConfiguration<T>
            where T : class, new()
        {
            configuration.UseFastCache = enabled;
            return configuration;
        }
    }
}
