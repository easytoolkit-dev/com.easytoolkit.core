using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyToolKit.Core.Patterns.Implementations
{
    /// <summary>
    /// Central registry for tracking all singleton instances.
    /// Provides thread-safe registration, unregistration, and querying of singletons.
    /// </summary>
    internal sealed class SingletonRegistry : ISingletonRegistry
    {
        private readonly Dictionary<Type, ILifecycleSingleton> _singletons = new();
        private readonly object _lock = new();

        /// <summary>
        /// Registers a singleton instance with the registry.
        /// </summary>
        /// <param name="singleton">The singleton instance to register.</param>
        /// <param name="type">The type key for the singleton.</param>
        /// <exception cref="SingletonInitializationException">Thrown if the type is already registered.</exception>
        public void Register(ILifecycleSingleton singleton, Type type)
        {
            lock (_lock)
            {
                if (_singletons.ContainsKey(type))
                    throw new SingletonInitializationException(
                        $"[SingletonRegistry] DuplicateRegistration: Type '{type.Name}' is already registered. " +
                        $"Only one instance per singleton type is allowed.",
                        type);

                _singletons[type] = singleton;
            }
        }

        /// <summary>
        /// Unregisters a singleton instance from the registry.
        /// </summary>
        /// <param name="singleton">The singleton instance to unregister.</param>
        public void Unregister(ILifecycleSingleton singleton)
        {
            lock (_lock)
            {
                var key = _singletons.FirstOrDefault(kvp => kvp.Value == singleton).Key;
                if (key != null)
                    _singletons.Remove(key);
            }
        }

        /// <summary>
        /// Gets a singleton instance by type.
        /// </summary>
        /// <param name="type">The type of singleton to retrieve.</param>
        /// <returns>The singleton instance, or null if not found.</returns>
        public ILifecycleSingleton Get(Type type)
        {
            lock (_lock)
            {
                return _singletons.TryGetValue(type, out var instance) ? instance : null;
            }
        }

        /// <summary>
        /// Gets all registered singleton instances.
        /// </summary>
        /// <returns>A read-only collection of all registered singletons.</returns>
        public IReadOnlyCollection<ILifecycleSingleton> GetAll()
        {
            lock (_lock)
            {
                return _singletons.Values.ToList().AsReadOnly();
            }
        }

        /// <summary>
        /// Checks if a singleton type is registered.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if registered, false otherwise.</returns>
        public bool IsRegistered(Type type)
        {
            lock (_lock)
            {
                return _singletons.ContainsKey(type);
            }
        }
    }
}
