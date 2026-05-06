using System;
using System.Collections.Generic;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Interface for singleton registration and tracking.
    /// </summary>
    public interface ISingletonRegistry
    {
        /// <summary>
        /// Registers a singleton instance with the registry.
        /// </summary>
        /// <param name="singleton">The singleton instance to register.</param>
        /// <param name="type">The type key for the singleton.</param>
        void Register(ILifecycleSingleton singleton, Type type);

        /// <summary>
        /// Unregisters a singleton instance from the registry.
        /// </summary>
        /// <param name="singleton">The singleton instance to unregister.</param>
        void Unregister(ILifecycleSingleton singleton);

        /// <summary>
        /// Gets a singleton instance by type.
        /// </summary>
        /// <param name="type">The type of singleton to retrieve.</param>
        /// <returns>The singleton instance, or null if not found.</returns>
        ILifecycleSingleton Get(Type type);

        /// <summary>
        /// Gets all registered singleton instances.
        /// </summary>
        /// <returns>A read-only collection of all registered singletons.</returns>
        IReadOnlyCollection<ILifecycleSingleton> GetAll();

        /// <summary>
        /// Checks if a singleton type is registered.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if registered, false otherwise.</returns>
        bool IsRegistered(Type type);
    }
}
