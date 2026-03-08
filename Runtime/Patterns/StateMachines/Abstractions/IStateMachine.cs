using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Full interface for a State Machine with read-only and mutation capabilities.
    /// </summary>
    public interface IStateMachine : IStateView
    {
        /// <summary>
        /// Adds a state to the state machine.
        /// </summary>
        /// <param name="keyName">The key name for the state.</param>
        /// <param name="state">The state instance.</param>
        void AddState(string keyName, IState state);

        /// <summary>
        /// Removes a state from the state machine.
        /// </summary>
        /// <param name="keyName">The key name of the state to remove.</param>
        void RemoveState(string keyName);

        /// <summary>
        /// Starts the state machine with the specified state.
        /// Can only be called when the state machine has not been started yet.
        /// </summary>
        /// <param name="keyName">The key name of the starting state.</param>
        void StartState(string keyName);

        /// <summary>
        /// Changes the current state to the state with the specified key.
        /// </summary>
        /// <param name="keyName">The key name of the target state.</param>
        void ChangeState(string keyName);

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
