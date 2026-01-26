using System;

namespace EasyToolKit.Core.Pooling.Implementations
{
    /// <summary>
    /// A fast cache structure using predefined static fields for rapid object retrieval and storage.
    /// This cache provides zero-allocation access to frequently used objects without collection operations.
    /// All slots are pre-allocated during construction and use boolean flags to track availability.
    /// </summary>
    /// <typeparam name="T">The type of objects to cache.</typeparam>
    public class FastCache<T> where T : class
    {
        /// <summary>
        /// The pre-allocated object slots.
        /// </summary>
        private readonly T _slot0;
        private readonly T _slot1;
        private readonly T _slot2;
        private readonly T _slot3;

        /// <summary>
        /// Flags indicating whether each slot is idle (true) or active (false).
        /// </summary>
        private bool _slot0Idle;
        private bool _slot1Idle;
        private bool _slot2Idle;
        private bool _slot3Idle;

        /// <summary>
        /// Gets the number of idle objects in the cache.
        /// </summary>
        public int IdleCount
        {
            get
            {
                int count = 0;
                if (_slot0Idle) count++;
                if (_slot1Idle) count++;
                if (_slot2Idle) count++;
                if (_slot3Idle) count++;
                return count;
            }
        }

        public int ActiveCount => 4 - IdleCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="FastCache{T}"/> structure.
        /// All slots are pre-allocated and marked as idle.
        /// </summary>
        /// <param name="allocator">The function used to allocate objects for each slot.</param>
        /// <exception cref="ArgumentNullException">Thrown when allocator is null.</exception>
        public FastCache(Func<T> allocator)
        {
            if (allocator == null)
            {
                throw new ArgumentNullException(nameof(allocator));
            }

            _slot0 = allocator();
            _slot1 = allocator();
            _slot2 = allocator();
            _slot3 = allocator();

            _slot0Idle = true;
            _slot1Idle = true;
            _slot2Idle = true;
            _slot3Idle = true;
        }

        /// <summary>
        /// Attempts to retrieve an idle object from the cache.
        /// </summary>
        /// <param name="result">The retrieved object, or <c>null</c> if no idle slot is available.</param>
        /// <returns><c>true</c> if an object was retrieved; otherwise, <c>false</c>.</returns>
        public bool TryGet(out T result)
        {
            if (_slot0Idle)
            {
                _slot0Idle = false;
                result = _slot0;
                return true;
            }

            if (_slot1Idle)
            {
                _slot1Idle = false;
                result = _slot1;
                return true;
            }

            if (_slot2Idle)
            {
                _slot2Idle = false;
                result = _slot2;
                return true;
            }

            if (_slot3Idle)
            {
                _slot3Idle = false;
                result = _slot3;
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Attempts to return an object to the cache.
        /// </summary>
        /// <param name="item">The object to return.</param>
        /// <returns><c>true</c> if the object was returned to cache; otherwise, <c>false</c> if the object is not from this cache.</returns>
        public bool TryPut(T item)
        {
            if (item == null)
            {
                return false;
            }

            if (ReferenceEquals(_slot0, item) && !_slot0Idle)
            {
                _slot0Idle = true;
                return true;
            }

            if (ReferenceEquals(_slot1, item) && !_slot1Idle)
            {
                _slot1Idle = true;
                return true;
            }

            if (ReferenceEquals(_slot2, item) && !_slot2Idle)
            {
                _slot2Idle = true;
                return true;
            }

            if (ReferenceEquals(_slot3, item) && !_slot3Idle)
            {
                _slot3Idle = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the cache contains the specified object.
        /// </summary>
        /// <param name="item">The object to locate.</param>
        /// <returns><c>true</c> if the object is from this cache; otherwise, <c>false</c>.</returns>
        public bool Contains(T item)
        {
            if (item == null)
            {
                return false;
            }

            return ReferenceEquals(_slot0, item)
                || ReferenceEquals(_slot1, item)
                || ReferenceEquals(_slot2, item)
                || ReferenceEquals(_slot3, item);
        }

        /// <summary>
        /// Determines whether the specified object is currently idle.
        /// </summary>
        /// <param name="item">The object to check.</param>
        /// <returns><c>true</c> if the object is idle; otherwise, <c>false</c>.</returns>
        public bool IsIdle(T item)
        {
            if (item == null)
            {
                return false;
            }

            if (ReferenceEquals(_slot0, item))
            {
                return _slot0Idle;
            }

            if (ReferenceEquals(_slot1, item))
            {
                return _slot1Idle;
            }

            if (ReferenceEquals(_slot2, item))
            {
                return _slot2Idle;
            }

            if (ReferenceEquals(_slot3, item))
            {
                return _slot3Idle;
            }

            return false;
        }

        /// <summary>
        /// Resets all slots to idle state.
        /// </summary>
        public void Reset()
        {
            _slot0Idle = true;
            _slot1Idle = true;
            _slot2Idle = true;
            _slot3Idle = true;
        }
    }
}
