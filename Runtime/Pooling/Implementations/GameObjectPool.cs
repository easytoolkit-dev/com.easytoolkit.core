using System;
using System.Collections.Generic;
using EasyToolkit.Core.Pooling;
using UnityEngine;

namespace EasyToolkit.Core.Pooling.Implementations
{
    /// <summary>
    /// Implementation of a specialized object pool for Unity GameObject instances.
    /// </summary>
    internal sealed class GameObjectPool : PoolBase<GameObject>, IGameObjectPool, IGameObjectPoolTicker
    {
        private readonly Dictionary<GameObject, PooledGameObjectInfo> _activeInstances =
            new Dictionary<GameObject, PooledGameObjectInfo>();

        private readonly List<PooledGameObjectInfo> _idleInstances = new List<PooledGameObjectInfo>();

        private readonly GameObject _original;
        private readonly Transform _rootTransform;
        private readonly GameObjectPoolConfiguration _configuration;

        private float _tickElapsedTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObjectPool"/> class.
        /// </summary>
        /// <param name="name">The name of the pool.</param>
        /// <param name="original">The original prefab for instantiation.</param>
        /// <param name="configuration">The configuration for the pool.</param>
        /// <param name="rootTransform">The root Transform for pooled objects.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="name"/>, <paramref name="original"/>, or <paramref name="configuration"/> is null.
        /// </exception>
        public GameObjectPool(string name, GameObject original, GameObjectPoolConfiguration configuration, Transform rootTransform)
            : base(name)
        {
            _original = original ?? throw new ArgumentNullException(nameof(original));
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _rootTransform = rootTransform;
            _configuration = configuration;

            // Preallocate instances if specified
            if (configuration.PreallocationCount > 0)
            {
                PreallocateInstances(configuration.PreallocationCount);
            }

            // Set capacity if specified
            if (configuration.MaxCapacity.HasValue)
            {
                Capacity = configuration.MaxCapacity.Value;
            }
        }

        /// <inheritdoc />
        public GameObject Original => _original;

        /// <inheritdoc />
        public Transform Transform => _rootTransform;

        /// <inheritdoc />
        public override int ActiveCount => _activeInstances.Count;

        /// <inheritdoc />
        public override int IdleCount => _idleInstances.Count;

        /// <inheritdoc />
        protected override GameObject RentFromIdle()
        {
            PooledGameObjectInfo pooledInfo;

            if (_idleInstances.Count > 0)
            {
                int lastIndex = _idleInstances.Count - 1;
                pooledInfo = _idleInstances[lastIndex];
                _idleInstances.RemoveAt(lastIndex);
            }
            else
            {
                var instance = UnityEngine.Object.Instantiate(_original, _rootTransform);
                pooledInfo = new PooledGameObjectInfo(instance, this)
                {
                    ActiveLifetime = _configuration.ActiveLifetime,
                    IdleLifetime = _configuration.IdleLifetime,
                };
            }

            _activeInstances.Add(pooledInfo.Target, pooledInfo);

            pooledInfo.State = PooledGameObjectState.Active;
            pooledInfo.ElapsedTime = 0f;

            if (_configuration.EnablePoolItemCallbacks)
            {
                foreach (var item in pooledInfo.PoolItems)
                {
                    item.OnRent();
                }
            }

            return pooledInfo.Target;
        }

        /// <inheritdoc />
        protected override bool ReleaseToIdle(GameObject instance)
        {
            if (!_activeInstances.TryGetValue(instance, out var pooledInfo))
            {
                return false;
            }

            pooledInfo.State = PooledGameObjectState.Idle;
            pooledInfo.ElapsedTime = 0f;

            instance.transform.SetParent(_rootTransform, false);

            _activeInstances.Remove(instance);
            _idleInstances.Add(pooledInfo);

            if (_configuration.EnablePoolItemCallbacks)
            {
                foreach (var item in pooledInfo.PoolItems)
                {
                    item.OnRelease();
                }
            }

            return true;
        }

        /// <inheritdoc />
        protected override bool RemoveFromActive(GameObject instance)
        {
            return _activeInstances.Remove(instance);
        }

        /// <inheritdoc />
        protected override void ShrinkIdleObjectsToFitCapacity(int shrinkCount)
        {
            int removeCount = Math.Min(shrinkCount, _idleInstances.Count);

            for (int i = 0; i < removeCount; i++)
            {
                var pooledInfo = _idleInstances[i];
                UnityEngine.Object.Destroy(pooledInfo.Target);
            }

            _idleInstances.RemoveRange(0, removeCount);
        }

        /// <inheritdoc />
        protected override void ClearManagedObjects()
        {
            foreach (var pooledInfo in _activeInstances.Values)
            {
                DestroyGameObject(pooledInfo.Target);
            }

            for (int i = 0; i < _idleInstances.Count; i++)
            {
                DestroyGameObject(_idleInstances[i].Target);
            }

            _activeInstances.Clear();
            _idleInstances.Clear();
            _tickElapsedTime = 0f;
        }

        /// <inheritdoc />
        public IPooledGameObjectLifetimeAccessor GetLifetimeAccessor(GameObject instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var pooledInfo = GetPooledInfo(instance);
            if (pooledInfo == null)
            {
                throw new ArgumentException(
                    $"The specified instance '{instance.name}' is not managed by this object pool '{Name}'.",
                    nameof(instance));
            }

            return new PooledGameObjectLifetimeAccessor(
                () => pooledInfo.ActiveLifetime,
                value => pooledInfo.ActiveLifetime = value,
                () => pooledInfo.IdleLifetime,
                value => pooledInfo.IdleLifetime = value,
                () => pooledInfo.ElapsedTime,
                value => pooledInfo.ElapsedTime = value);
        }

        /// <inheritdoc />
        public bool TryGetLifetimeAccessor(GameObject instance, out IPooledGameObjectLifetimeAccessor lifetimeAccessor)
        {
            if (instance == null)
            {
                lifetimeAccessor = null;
                return false;
            }

            var pooledInfo = GetPooledInfo(instance);
            if (pooledInfo == null)
            {
                lifetimeAccessor = null;
                return false;
            }

            lifetimeAccessor = new PooledGameObjectLifetimeAccessor(
                () => pooledInfo.ActiveLifetime,
                value => pooledInfo.ActiveLifetime = value,
                () => pooledInfo.IdleLifetime,
                value => pooledInfo.IdleLifetime = value,
                () => pooledInfo.ElapsedTime,
                value => pooledInfo.ElapsedTime = value);

            return true;
        }

        /// <inheritdoc />
        void IGameObjectPoolTicker.OnTick(float deltaTime)
        {
            _tickElapsedTime += deltaTime;

            while (_tickElapsedTime >= _configuration.TickInterval)
            {
                OnIntervalTick(_configuration.TickInterval);
                _tickElapsedTime -= _configuration.TickInterval;
            }
        }

        private void PreallocateInstances(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var instance = UnityEngine.Object.Instantiate(_original, _rootTransform);
                instance.SetActive(false);

                var pooledInfo = new PooledGameObjectInfo(instance, this)
                {
                    ActiveLifetime = _configuration.ActiveLifetime,
                    IdleLifetime = _configuration.IdleLifetime,
                    State = PooledGameObjectState.Idle,
                    ElapsedTime = 0f
                };

                _idleInstances.Add(pooledInfo);
            }
        }

        private void OnIntervalTick(float interval)
        {
            var pendingRecycleInstances = new List<PooledGameObjectInfo>();

            // Process active instances
            foreach (var kvp in _activeInstances)
            {
                var pooledInfo = kvp.Value;
                if (pooledInfo.ActiveLifetime.HasValue)
                {
                    pooledInfo.ElapsedTime += interval;

                    if (pooledInfo.ElapsedTime >= pooledInfo.ActiveLifetime.Value)
                    {
                        pendingRecycleInstances.Add(pooledInfo);
                    }
                }
            }

            // Recycle instances that exceeded active lifetime
            foreach (var pooledInfo in pendingRecycleInstances)
            {
                Release(pooledInfo.Target);
            }

            // Process idle instances - filter out expired ones in O(n)
            // Update elapsed time for all idle instances and mark expired ones
            for (int i = _idleInstances.Count - 1; i >= 0; i--)
            {
                var pooledInfo = _idleInstances[i];
                if (pooledInfo.IdleLifetime.HasValue)
                {
                    pooledInfo.ElapsedTime += interval;

                    if (pooledInfo.ElapsedTime >= pooledInfo.IdleLifetime.Value)
                    {
                        if (Application.isPlaying)
                        {
                            UnityEngine.Object.Destroy(pooledInfo.Target);
                        }
                        else
                        {
                            UnityEngine.Object.DestroyImmediate(pooledInfo.Target);
                        }
                        _idleInstances.RemoveAt(i);
                    }
                }
            }
        }

        private PooledGameObjectInfo GetPooledInfo(GameObject instance)
        {
            if (_activeInstances.TryGetValue(instance, out var pooledInfo))
            {
                return pooledInfo;
            }

            foreach (var idleInfo in _idleInstances)
            {
                if (idleInfo.Target == instance)
                {
                    return idleInfo;
                }
            }

            return null;
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
