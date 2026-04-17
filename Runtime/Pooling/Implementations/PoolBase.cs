using System;

namespace EasyToolkit.Core.Pooling.Implementations
{
    /// <summary>
    /// Abstract base class for object pool implementations.
    /// </summary>
    /// <typeparam name="T">The type of objects managed by the pool.</typeparam>
    internal abstract class PoolBase<T> : IPool<T>
    {
        private readonly string _name;
        private int? _capacity;

        /// <summary>
        /// Initializes a new instance of the <see cref="PoolBase{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the pool.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null.</exception>
        protected PoolBase(string name)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <inheritdoc />
        public string Name => _name;

        /// <inheritdoc />
        public abstract int ActiveCount { get; }

        /// <inheritdoc />
        public abstract int IdleCount { get; }

        /// <summary>
        /// Gets the total number of objects managed by this pool.
        /// </summary>
        public int TotalCount => ActiveCount + IdleCount;

        /// <inheritdoc />
        public int? Capacity
        {
            get => _capacity;
            set
            {
                if (_capacity == value)
                {
                    return;
                }

                if (value.HasValue && value.Value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Capacity cannot be negative.");
                }

                if (value.HasValue && value.Value < ActiveCount)
                {
                    throw new InvalidOperationException(
                        $"Capacity '{value.Value}' cannot be less than active object count '{ActiveCount}'.");
                }

                _capacity = value;

                if (_capacity.HasValue && TotalCount > _capacity.Value)
                {
                    ShrinkIdleObjectsToFitCapacity(TotalCount - _capacity.Value);
                }
            }
        }

        /// <inheritdoc />
        public T Rent()
        {
            if (_capacity.HasValue && TotalCount >= _capacity.Value && IdleCount == 0)
            {
                throw new InvalidOperationException(
                    $"No idle object available in pool '{Name}'. " +
                    $"Pool has reached capacity limit of {_capacity.Value}.");
            }

            var instance = RentFromIdle();
            OnRent(instance);
            return instance;
        }

        /// <inheritdoc />
        public bool Release(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            bool released = ReleaseToIdle(instance);

            if (released)
            {
                OnRelease(instance);
            }

            return released;
        }

        /// <inheritdoc />
        public bool Remove(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return RemoveFromActive(instance);
        }

        /// <inheritdoc />
        public void Clear()
        {
            ClearManagedObjects();
        }

        /// <summary>
        /// Rents an instance from the idle collection or creates a new one.
        /// </summary>
        /// <returns>The rented instance.</returns>
        protected abstract T RentFromIdle();

        /// <summary>
        /// Releases an instance back to the idle collection.
        /// </summary>
        /// <param name="instance">The instance to release.</param>
        /// <returns><c>true</c> if successfully released; otherwise, <c>false</c>.</returns>
        protected abstract bool ReleaseToIdle(T instance);

        /// <summary>
        /// Removes an instance from the active collection.
        /// </summary>
        /// <param name="instance">The instance to remove.</param>
        /// <returns><c>true</c> if successfully removed; otherwise, <c>false</c>.</returns>
        protected abstract bool RemoveFromActive(T instance);

        /// <summary>
        /// Shrinks the idle collection to fit within the capacity constraint.
        /// </summary>
        /// <param name="shrinkCount">The number of idle instances to remove.</param>
        protected abstract void ShrinkIdleObjectsToFitCapacity(int shrinkCount);

        /// <summary>
        /// Clears all objects currently managed by this pool.
        /// </summary>
        protected abstract void ClearManagedObjects();

        /// <summary>
        /// Called when an object is rented from the pool.
        /// Override to provide custom behavior.
        /// </summary>
        /// <param name="instance">The rented instance.</param>
        protected virtual void OnRent(T instance) { }

        /// <summary>
        /// Called when an object is released back to the pool.
        /// Override to provide custom behavior.
        /// </summary>
        /// <param name="instance">The released instance.</param>
        protected virtual void OnRelease(T instance) { }
    }
}
