using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Read-only view of a generic State Machine.
    /// Provides query operations and state change observation without mutation capabilities.
    /// </summary>
    public interface IStateView
    {
        /// <summary>
        /// Gets the current active state instance.
        /// </summary>
        IState CurrentState { get; }

        /// <summary>
        /// Gets the key of the current active state.
        /// </summary>
        string CurrentStateKey { get; }

        /// <summary>
        /// Event triggered when the state changes.
        /// </summary>
        event StateChangeHandler StateChanged;

        /// <summary>
        /// Finds a state by its key.
        /// </summary>
        /// <param name="keyName">The key name to look for.</param>
        /// <returns>The state instance if found, otherwise null.</returns>
        IState FindState(string keyName);
    }
}
