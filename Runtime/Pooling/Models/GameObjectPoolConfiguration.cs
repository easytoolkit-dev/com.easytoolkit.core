using System;

namespace EasyToolkit.Core.Pooling
{
    /// <summary>
    /// Configuration for GameObject pools.
    /// Provides properties for configuring GameObject pool behavior.
    /// </summary>
    public sealed class GameObjectPoolConfiguration : PoolConfigurationBase
    {
        public GameObjectPoolConfiguration(
            int preallocationCount = 0,
            int? maxCapacity = null,
            bool enablePoolItemCallbacks = true,
            float? activeLifetime = null,
            float? idleLifetime = 10f,
            float tickInterval = 1f)
            : base(preallocationCount, maxCapacity, enablePoolItemCallbacks)
        {
            if (activeLifetime.HasValue && activeLifetime.Value < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(activeLifetime), "Active lifetime cannot be negative.");
            }

            if (idleLifetime.HasValue && idleLifetime.Value < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(idleLifetime), "Idle lifetime cannot be negative.");
            }

            ActiveLifetime = activeLifetime;
            IdleLifetime = idleLifetime;
            TickInterval = tickInterval;
        }

        /// <summary>
        /// Gets or sets the maximum lifetime for active objects.
        /// Objects exceeding this time will be recycled back to the pool.
        /// A <c>null</c> value disables automatic recycling.
        /// </summary>
        public float? ActiveLifetime { get; }

        /// <summary>
        /// Gets or sets the maximum lifetime for idle objects.
        /// Objects exceeding this time will be destroyed.
        /// A <c>null</c> value disables automatic destruction.
        /// </summary>
        public float? IdleLifetime { get; }

        /// <summary>
        /// Gets or sets the interval between tick updates (in seconds).
        /// </summary>
        public float TickInterval { get; }
    }
}
