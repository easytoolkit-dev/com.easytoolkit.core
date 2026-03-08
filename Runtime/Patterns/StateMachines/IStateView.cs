using System;

namespace EasyToolkit.Core.Patterns
{
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
        IState<T> CurrentState { get; }

        /// <summary>
        /// Gets the key of the current active state.
        /// </summary>
        T? CurrentStateKey { get; }

        /// <summary>
        /// Finds a state by its key.
        /// </summary>
        /// <param name="key">The enum key to look for.</param>
        /// <returns>The state instance if found, otherwise null.</returns>
        IState<T> FindState(T key);

        /// <summary>
        /// Event triggered when the state changes.
        /// </summary>
        event StateChangeHandler<T> StateChanged;
    }
}
