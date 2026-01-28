using System;
using System.Collections.Generic;
using EasyToolkit.Core.Pooling;
using JetBrains.Annotations;

namespace EasyToolkit.Core.Pooling.Implementations
{
    /// <summary>
    /// Implementation of a generic object pool for C# objects.
    /// </summary>
    /// <typeparam name="T">The type of objects to pool.</typeparam>
    public class ObjectPool<T> : PoolBase<T>, IObjectPool<T> where T : class, new()
    {
        private readonly HashSet<T> _activeInstances = new HashSet<T>();
        private readonly Stack<T> _idleInstances = new Stack<T>();
        private readonly Func<T> _allocator;
        private readonly bool _callPoolItemCallbacks;
        [CanBeNull] private readonly FastCache<T> _fastCache;

        private Action<T> _onRent;
        private Action<T> _onRelease;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPool{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the pool.</param>
        /// <param name="configuration">The configuration for the pool.</param>
        public ObjectPool(string name, ObjectPoolConfiguration<T> configuration) : base(name)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configuration.Validate();

            _allocator = configuration.Allocator ?? (() => new T());
            _callPoolItemCallbacks = configuration.CallPoolItemCallbacks;

            if (configuration.UseFastCache)
            {
                _fastCache = new FastCache<T>(_allocator);
            }

            if (configuration.InitialCapacity > 0)
            {
                PreallocateInstances(configuration.InitialCapacity);
            }

            if (configuration.MaxCapacity >= 0)
            {
                Capacity = configuration.MaxCapacity;
            }
        }

        /// <inheritdoc />
        public override int ActiveCount => _activeInstances.Count + (_fastCache?.ActiveCount ?? 0);

        /// <inheritdoc />
        public override int IdleCount => _idleInstances.Count + (_fastCache?.IdleCount ?? 0);

        /// <inheritdoc />
        public Type ObjectType => typeof(T);

        /// <inheritdoc />
        public void AddRentCallback(Action<T> callback)
        {
            _onRent += callback;
        }

        /// <inheritdoc />
        public void AddReleaseCallback(Action<T> callback)
        {
            _onRelease += callback;
        }

        /// <inheritdoc />
        protected override T RentFromIdle()
        {
            // Try FastCache first (L0 cache)
            if (_fastCache != null && _fastCache.TryGet(out var instance))
            {
                return instance;
            }

            // Fall back to Stack (L1 cache)
            if (_idleInstances.Count > 0)
            {
                instance = _idleInstances.Pop();
            }
            else
            {
                instance = _allocator();
            }

            _activeInstances.Add(instance);
            return instance;
        }

        /// <inheritdoc />
        protected override bool ReleaseToIdle(T instance)
        {
            if (instance == null)
            {
                return false;
            }

            // Try to return to FastCache first (L0 cache)
            if (_fastCache != null && _fastCache.TryPut(instance))
            {
                return true;
            }

            if (!_activeInstances.Remove(instance))
            {
                return false; // Already idle or not from this pool
            }

            // Fall back to Stack (L1 cache)
            _idleInstances.Push(instance);
            return true;
        }

        /// <inheritdoc />
        protected override bool RemoveFromActive(T instance)
        {
            return _activeInstances.Remove(instance);
        }

        /// <inheritdoc />
        protected override void ShrinkIdleObjectsToFitCapacity(int shrinkCount)
        {
            // FastCache has fixed pre-allocated slots, so we only shrink from Stack (L1 cache)
            for (int i = 0; i < shrinkCount && _idleInstances.Count > 0; i++)
            {
                _idleInstances.Pop();
            }
        }

        /// <inheritdoc />
        protected override void OnRent(T instance)
        {
            if (_callPoolItemCallbacks && instance is IPoolItem poolItem)
            {
                poolItem.Rent();
            }

            _onRent?.Invoke(instance);
        }

        /// <inheritdoc />
        protected override void OnRelease(T instance)
        {
            if (_callPoolItemCallbacks && instance is IPoolItem poolItem)
            {
                poolItem.Release();
            }

            _onRelease?.Invoke(instance);
        }

        private void PreallocateInstances(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _idleInstances.Push(_allocator());
            }
        }
    }
}
