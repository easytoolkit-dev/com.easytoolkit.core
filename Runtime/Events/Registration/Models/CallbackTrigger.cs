using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Base class for Unity MonoBehaviour triggers that manage callback lifecycle.
    /// </summary>
    /// <remarks>
    /// This class maintains a thread-safe collection of callbacks and provides functionality
    /// to batch execute them. Derived classes trigger callback execution based on specific
    /// Unity lifecycle events. All operations are protected by locks for thread safety.
    /// If a callback throws an exception, it will be caught and logged without affecting
    /// the execution of other callbacks.
    /// </remarks>
    public abstract class CallbackTrigger : MonoBehaviour
    {
        private readonly HashSet<Action> _callbacks = new HashSet<Action>();
        private readonly object _lock = new();

        /// <summary>
        /// Adds a callback to the trigger's managed collection.
        /// </summary>
        /// <param name="callback">The callback to add.</param>
        /// <remarks>
        /// Adding the same callback multiple times has no additional effect due to
        /// the use of a HashSet for storage. This method is thread-safe.
        /// Null callbacks are silently ignored.
        /// </remarks>
        public void AddCallback(Action callback)
        {
            if (callback == null)
                return;

            lock (_lock)
            {
                _callbacks.Add(callback);
            }
        }

        /// <summary>
        /// Removes a callback from the trigger's managed collection.
        /// </summary>
        /// <param name="callback">The callback to remove.</param>
        /// <returns>True if the callback was found and removed; otherwise, false.</returns>
        /// <remarks>
        /// This method is thread-safe. Null callbacks return false.
        /// </remarks>
        public bool RemoveCallback(Action callback)
        {
            if (callback == null)
                return false;

            lock (_lock)
            {
                return _callbacks.Remove(callback);
            }
        }

        /// <summary>
        /// Executes all currently managed callbacks and clears the internal collection.
        /// </summary>
        /// <remarks>
        /// This method calls each callback in the order they were added (HashSet iteration order)
        /// and then clears the internal collection. Calling this method multiple times is safe
        /// as subsequent calls will have no effect. This method is thread-safe.
        /// If a callback throws an exception, it will be caught, logged, and will not prevent
        /// other callbacks from executing.
        /// </remarks>
        public void ExecuteCallbacks()
        {
            lock (_lock)
            {
                foreach (var callback in _callbacks)
                {
                    try
                    {
                        callback?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error executing callback: {ex.Message}\n{ex.StackTrace}");
                    }
                }

                _callbacks.Clear();
            }
        }
    }
}
