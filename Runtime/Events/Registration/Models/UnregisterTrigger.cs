using System.Collections.Generic;
using UnityEngine;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Base class for Unity MonoBehaviour triggers that manage event registration lifecycle.
    /// </summary>
    /// <remarks>
    /// This class maintains a thread-safe collection of event registrations and provides functionality
    /// to batch unregister them. Derived classes trigger unregistration based on specific
    /// Unity lifecycle events. All operations are protected by locks for thread safety.
    /// </remarks>
    public abstract class UnregisterTrigger : MonoBehaviour
    {
        private readonly HashSet<IRegistration> _registrations = new HashSet<IRegistration>();
        private readonly object _lock = new();

        /// <summary>
        /// Adds a registration to the trigger's managed collection.
        /// </summary>
        /// <param name="unregister">The registration to add.</param>
        /// <remarks>
        /// Adding the same registration multiple times has no additional effect due to
        /// the use of a HashSet for storage. This method is thread-safe.
        /// </remarks>
        public void AddUnregister(IRegistration unregister)
        {
            lock (_lock)
            {
                _registrations.Add(unregister);
            }
        }

        /// <summary>
        /// Removes a registration from the trigger's managed collection.
        /// </summary>
        /// <param name="unregister">The registration to remove.</param>
        /// <returns>True if the registration was found and removed; otherwise, false.</returns>
        /// <remarks>This method is thread-safe.</remarks>
        public bool RemoveUnregister(IRegistration unregister)
        {
            lock (_lock)
            {
                return _registrations.Remove(unregister);
            }
        }

        /// <summary>
        /// Unregisters all currently managed event registrations.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="IRegistration.Unregister"/> on each managed registration
        /// and then clears the internal collection. Calling this method multiple times is safe
        /// as subsequent calls will have no effect. This method is thread-safe.
        /// </remarks>
        public void Unregister()
        {
            lock (_lock)
            {
                foreach (var registration in _registrations)
                {
                    registration.Unregister();
                }

                _registrations.Clear();
            }
        }
    }
}
