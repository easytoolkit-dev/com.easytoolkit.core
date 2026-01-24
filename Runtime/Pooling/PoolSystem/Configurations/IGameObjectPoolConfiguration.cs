namespace EasyToolKit.Core.Pooling
{
    /// <summary>
    /// Configuration interface for GameObject pools.
    /// Provides mutable builder properties for GameObject pool configuration.
    /// </summary>
    public interface IGameObjectPoolConfiguration : IPoolConfiguration
    {
        /// <summary>
        /// Gets or sets the default maximum lifetime for active objects.
        /// Objects exceeding this time will be recycled back to the pool.
        /// Values less than zero indicate unlimited lifetime.
        /// </summary>
        float DefaultActiveLifetime { get; set; }

        /// <summary>
        /// Gets or sets the default maximum lifetime for idle objects.
        /// Objects exceeding this time will be destroyed.
        /// Values less than zero indicate unlimited lifetime.
        /// </summary>
        float DefaultIdleLifetime { get; set; }

        /// <summary>
        /// Gets or sets the interval between tick updates (in seconds).
        /// </summary>
        float TickInterval { get; set; }
    }
}
