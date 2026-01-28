using System;

namespace EasyToolkit.Core.Pooling
{
    /// <summary>
    /// Configuration for GameObject pools.
    /// Provides properties for configuring GameObject pool behavior.
    /// </summary>
    public sealed class GameObjectPoolConfiguration : PoolConfigurationBase
    {

        /// <summary>
        /// Gets or sets the default maximum lifetime for active objects.
        /// Objects exceeding this time will be recycled back to the pool.
        /// Values less than zero indicate unlimited lifetime.
        /// </summary>
        public float DefaultActiveLifetime { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the default maximum lifetime for idle objects.
        /// Objects exceeding this time will be destroyed.
        /// Values less than zero indicate unlimited lifetime.
        /// </summary>
        public float DefaultIdleLifetime { get; set; } = 10f;

        /// <summary>
        /// Gets or sets the interval between tick updates (in seconds).
        /// </summary>
        public float TickInterval { get; set; } = 0.5f;

        /// <summary>
        /// Validates the current configuration.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when validation fails.
        /// </exception>
        public override void Validate()
        {
            base.Validate();

            if (TickInterval <= 0)
            {
                throw new InvalidOperationException(
                    $"TickInterval must be positive. Current value: {TickInterval}");
            }
        }
    }
}
