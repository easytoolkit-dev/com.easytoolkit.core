using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Full interface for a generic State Machine with read-only and mutation capabilities.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public interface IStateMachine<T> : IStateView<T> where T : struct, Enum
    {
        /// <summary>
        /// Adds a state to the state machine.
        /// </summary>
        /// <param name="key">The enum key for the state.</param>
        /// <param name="state">The state instance.</param>
        void AddState(T key, IState<T> state);

        /// <summary>
        /// Removes a state from the state machine.
        /// </summary>
        /// <param name="key">The enum key of the state to remove.</param>
        void RemoveState(T key);

        /// <summary>
        /// Starts the state machine with the specified state.
        /// Can only be called when the state machine has not been started yet.
        /// </summary>
        /// <param name="key">The enum key of the starting state.</param>
        void StartState(T key);

        /// <summary>
        /// Changes the current state to the state with the specified key.
        /// </summary>
        /// <param name="key">The enum key of the target state.</param>
        void ChangeState(T key);

        /// <summary>
        /// Updates the current state.
        /// Should be called every frame.
        /// </summary>
        void Update();

        /// <summary>
        /// Updates the current state.
        /// Should be called every fixed framerate frame.
        /// </summary>
        void FixedUpdate();
    }
}
