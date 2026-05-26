using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyToolkit.Core.Pooling
{
    /// <summary>
    /// Implementation of a manager for creating and managing GameObject pools.
    /// </summary>
    public sealed class GameObjectPoolManager : IGameObjectPoolTicker
    {
        private readonly Dictionary<string, IGameObjectPool> _pools = new();

        private readonly Transform _rootTransform;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObjectPoolManager"/> class.
        /// </summary>
        /// <param name="rootTransform">The root Transform for pooled GameObject hierarchy.</param>
        public GameObjectPoolManager(Transform rootTransform)
        {
            _rootTransform = rootTransform;
        }

        /// <summary>
        /// Gets the root Transform for pooled GameObject hierarchy.
        /// </summary>
        public Transform Transform => _rootTransform;

        /// <summary>
        /// Creates a new GameObject pool with default configuration.
        /// </summary>
        /// <param name="poolName">The name of the pool.</param>
        /// <param name="original">The original prefab for instantiation.</param>
        /// <returns>The newly created pool.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a pool with the same name already exists.
        /// </exception>
        public IGameObjectPool CreatePool(string poolName, GameObject original)
        {
            return CreatePool(poolName, original, new GameObjectPoolConfiguration());
        }

        /// <summary>
        /// Creates a new GameObject pool with default configuration.
        /// </summary>
        /// <param name="poolName">The name of the pool.</param>
        /// <param name="factory">The factory that creates new GameObject instances.</param>
        /// <returns>The newly created pool.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a pool with the same name already exists or the factory returns null.
        /// </exception>
        public IGameObjectPool CreatePool(string poolName, Func<GameObject> factory)
        {
            return CreatePool(poolName, factory, new GameObjectPoolConfiguration());
        }

        /// <summary>
        /// Creates a new GameObject pool with custom configuration.
        /// </summary>
        /// <param name="poolName">The name of the pool.</param>
        /// <param name="original">The original prefab for instantiation.</param>
        /// <param name="configuration">The configuration for the pool.</param>
        /// <returns>The newly created pool.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a pool with the same name already exists.
        /// </exception>
        public IGameObjectPool CreatePool(string poolName, GameObject original, GameObjectPoolConfiguration configuration)
        {
            if (poolName == null)
            {
                throw new ArgumentNullException(nameof(poolName));
            }

            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (_pools.ContainsKey(poolName))
            {
                throw new InvalidOperationException(
                    $"Game object pool '{poolName}' already exists.");
            }

            return CreatePoolCore(
                poolName,
                poolRoot => new GameObjectPool(poolName, original, configuration, poolRoot));
        }

        /// <summary>
        /// Creates a new GameObject pool with custom configuration.
        /// </summary>
        /// <param name="poolName">The name of the pool.</param>
        /// <param name="factory">The factory that creates new GameObject instances.</param>
        /// <param name="configuration">The configuration for the pool.</param>
        /// <returns>The newly created pool.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a pool with the same name already exists or the factory returns null.
        /// </exception>
        public IGameObjectPool CreatePool(string poolName, Func<GameObject> factory, GameObjectPoolConfiguration configuration)
        {
            if (poolName == null)
            {
                throw new ArgumentNullException(nameof(poolName));
            }

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (_pools.ContainsKey(poolName))
            {
                throw new InvalidOperationException(
                    $"Game object pool '{poolName}' already exists.");
            }

            return CreatePoolCore(
                poolName,
                poolRoot => new GameObjectPool(poolName, factory, configuration, poolRoot));
        }

        /// <summary>
        /// Gets the names of all managed pools.
        /// </summary>
        public IEnumerable<string> GetPoolNames()
        {
            return _pools.Keys;
        }

        /// <summary>
        /// Determines whether a pool with the specified name exists.
        /// </summary>
        /// <param name="poolName">The name of the pool.</param>
        /// <returns>
        /// <c>true</c> if a pool with the specified name exists; otherwise, <c>false</c>.
        /// </returns>
        public bool HasPool(string poolName)
        {
            if (poolName == null)
            {
                throw new ArgumentNullException(nameof(poolName));
            }

            return _pools.ContainsKey(poolName);
        }

        /// <summary>
        /// Tries to get the pool with the specified name.
        /// </summary>
        /// <param name="poolName">The name of the pool.</param>
        /// <param name="pool">
        /// When this method returns, contains the pool if found;
        /// otherwise, <c>null</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the pool was found; otherwise, <c>false</c>.
        /// </returns>
        public bool TryGetPool(string poolName, out IGameObjectPool pool)
        {
            if (poolName == null)
            {
                throw new ArgumentNullException(nameof(poolName));
            }

            return _pools.TryGetValue(poolName, out pool);
        }

        /// <summary>
        /// Gets the pool with the specified name.
        /// </summary>
        /// <param name="poolName">The name of the pool.</param>
        /// <returns>The pool with the specified name.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a pool with the specified name does not exist.
        /// </exception>
        public IGameObjectPool GetPool(string poolName)
        {
            if (poolName == null)
            {
                throw new ArgumentNullException(nameof(poolName));
            }

            if (_pools.TryGetValue(poolName, out var pool))
            {
                return pool;
            }

            throw new InvalidOperationException(
                $"Game object pool '{poolName}' does not exist.");
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
            if (poolName == null)
            {
                throw new ArgumentNullException(nameof(poolName));
            }

            if (!_pools.Remove(poolName, out var pool))
            {
                return false;
            }

            pool.Clear();

            if (pool.Transform != null)
            {
                if (Application.isPlaying)
                {
                    UnityEngine.Object.Destroy(pool.Transform.gameObject);
                }
                else
                {
                    UnityEngine.Object.DestroyImmediate(pool.Transform.gameObject);
                }
            }

            return true;
        }

        /// <inheritdoc />
        void IGameObjectPoolTicker.OnTick(float deltaTime)
        {
            foreach (var pool in _pools.Values)
            {
                if (pool is IGameObjectPoolTicker ticker)
                {
                    ticker.OnTick(deltaTime);
                }
            }
        }

        private IGameObjectPool CreatePoolCore(string poolName, Func<Transform, IGameObjectPool> poolFactory)
        {
            var poolRoot = new GameObject(poolName);
            poolRoot.transform.SetParent(_rootTransform, false);

            try
            {
                var pool = poolFactory(poolRoot.transform);
                _pools.Add(poolName, pool);
                return pool;
            }
            catch
            {
                DestroyGameObject(poolRoot);
                throw;
            }
        }

        private static void DestroyGameObject(GameObject target)
        {
            if (target == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                UnityEngine.Object.Destroy(target);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(target);
            }
        }
    }
}
