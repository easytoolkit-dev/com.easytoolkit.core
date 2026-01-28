using System;

namespace EasyToolkit.Core.Pooling
{
    /// <summary>
    /// Manages a collection of C# object pools.
    /// </summary>
    public interface IObjectPoolManager
    {
        /// <summary>
        /// Creates a new object pool with default configuration.
        /// </summary>
        /// <typeparam name="T">The type of objects to pool.</typeparam>
        /// <param name="poolName">The unique name for the pool.</param>
        /// <returns>The newly created pool.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a pool with the same name already exists.
        /// </exception>
        IObjectPool<T> CreatePool<T>(string poolName) where T : class, new();

        /// <summary>
        /// Creates a new object pool with custom configuration.
        /// </summary>
        /// <typeparam name="T">The type of objects to pool.</typeparam>
        /// <param name="poolName">The unique name for the pool.</param>
        /// <param name="configuration">The configuration for the pool.</param>
        /// <returns>The newly created pool.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a pool with the same name already exists.
        /// </exception>
        IObjectPool<T> CreatePool<T>(string poolName, ObjectPoolConfiguration<T> configuration)
            where T : class, new();

        /// <summary>
        /// Attempts to get the pool with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of objects in the pool.</typeparam>
        /// <param name="poolName">The name of the pool to retrieve.</param>
        /// <param name="pool">
        /// When this method returns, contains the pool if found;
        /// otherwise, <c>null</c>.
        /// </param>
        /// <returns><c>true</c> if the pool was found; otherwise, <c>false</c>.</returns>
        bool TryGetPool<T>(string poolName, out IObjectPool<T> pool)
            where T : class, new();
    }
}
