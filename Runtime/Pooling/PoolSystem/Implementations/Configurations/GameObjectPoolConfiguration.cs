using System;

namespace EasyToolKit.Core.Pooling.Implementations
{
    /// <summary>
    /// Configuration implementation for GameObject pools.
    /// Provides mutable builder properties and validation for GameObject pools.
    /// </summary>
    public class GameObjectPoolConfiguration : IGameObjectPoolConfiguration
    {
        /// <inheritdoc />
        public int InitialCapacity { get; set; }

        /// <inheritdoc />
        public int MaxCapacity { get; set; } = -1;

        /// <inheritdoc />
        public bool CallPoolItemCallbacks { get; set; } = true;

        /// <inheritdoc />
        public float DefaultActiveLifetime { get; set; } = 0f;

        /// <inheritdoc />
        public float DefaultIdleLifetime { get; set; } = 10f;

        /// <inheritdoc />
        public float TickInterval { get; set; } = 0.5f;

        /// <summary>
        /// Validates the current configuration.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when validation fails.
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

            if (TickInterval <= 0)
            {
                throw new InvalidOperationException(
                    $"TickInterval must be positive. Current value: {TickInterval}");
            }
        }
    }
}
