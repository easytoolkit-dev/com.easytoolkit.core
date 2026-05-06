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
        StateNode CurrentState { get; }

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
        StateNode FindState(string keyName);
    }

    /// <summary>
    /// Read-only view of a generic State Machine.
    /// Provides query operations and state change observation without mutation capabilities.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public interface IStateView<T> where T : struct, Enum
    {
        /// <summary>
        /// Gets the current active state instance.
        /// </summary>
        StateNode<T> CurrentState { get; }

        /// <summary>
        /// Gets the key of the current active state.
        /// </summary>
        T? CurrentStateKey { get; }

        /// <summary>
        /// Event triggered when the state changes.
        /// </summary>
        event StateChangeHandler<T> StateChanged;

        /// <summary>
        /// Finds a state by its key.
        /// </summary>
        /// <param name="key">The enum key to look for.</param>
        /// <returns>The state instance if found, otherwise null.</returns>
        StateNode<T> FindState(T key);
    }
}
