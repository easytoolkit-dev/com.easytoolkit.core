using System;
using System.Collections.Generic;
using EasyToolkit.Core.Pooling;

namespace EasyToolkit.Core.Pooling.Implementations
{
    /// <summary>
    /// Default implementation of <see cref="IObjectPoolManager"/>.
    /// </summary>
    internal sealed class ObjectPoolManager : IObjectPoolManager
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
        internal ObjectPoolManager()
        {
            _pools = new Dictionary<string, object>();
        }

        /// <inheritdoc />
        public IObjectPool<T> CreatePool<T>(string poolName) where T : class, new()
        {
            return CreatePool(poolName, new ObjectPoolConfiguration<T>());
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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
