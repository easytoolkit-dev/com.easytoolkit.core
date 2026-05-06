using System;
using System.Collections.Generic;

namespace EasyToolkit.Core.Pooling
{
    public sealed class ObjectPoolManager
    {
        private readonly Dictionary<string, object> _pools;

        /// <summary>
        /// Gets the number of pools managed by this manager.
        /// </summary>
        public int PoolCount => _pools.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPoolManager"/> class
        /// with the default factory.
        /// </summary>
        public ObjectPoolManager()
        {
            _pools = new Dictionary<string, object>();
        }

        /// <summary>
        /// Creates a new object pool with default configuration.
        /// </summary>
        /// <typeparam name="T">The type of objects to pool.</typeparam>
        /// <param name="poolName">The unique name for the pool.</param>
        /// <returns>The newly created pool.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a pool with the same name already exists.
        /// </exception>
        public IObjectPool<T> CreatePool<T>(string poolName) where T : class, new()
        {
            return CreatePool(poolName, new ObjectPoolConfiguration<T>());
        }

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
        public IObjectPool<T> CreatePool<T>(string poolName, ObjectPoolConfiguration<T> configuration)
            where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(poolName))
            {
                throw new ArgumentException("Pool name cannot be null or whitespace.", nameof(poolName));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (_pools.ContainsKey(poolName))
            {
                throw new InvalidOperationException(
                    $"Object pool with name '{poolName}' already exists.");
            }

            var pool = new ObjectPool<T>(poolName, configuration);
            _pools[poolName] = pool;
            return pool;
        }

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
        public bool TryGetPool<T>(string poolName, out IObjectPool<T> pool)
            where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(poolName))
            {
                throw new ArgumentException("Pool name cannot be null or whitespace.", nameof(poolName));
            }

            if (_pools.TryGetValue(poolName, out var poolObj) && poolObj is IObjectPool<T> typedPool)
            {
                pool = typedPool;
                return true;
            }

            pool = null;
            return false;
        }

        /// <summary>
        /// Removes the pool with the specified name from this manager.
        /// </summary>
        /// <param name="poolName">The name of the pool to remove.</param>
        /// <returns>
        /// <c>true</c> if the pool was removed; otherwise, <c>false</c>.
        /// </returns>
        public bool RemovePool(string poolName)
        {
            if (string.IsNullOrWhiteSpace(poolName))
            {
                throw new ArgumentException("Pool name cannot be null or whitespace.", nameof(poolName));
            }

            return _pools.Remove(poolName);
        }
    }
}
