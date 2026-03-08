using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Interface for a state in the State Machine.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public interface IState<T> where T : struct, Enum
    {
        /// <summary>
        /// Called when the state is entered.
        /// </summary>
        /// <param name="owner">The state machine that owns this state.</param>
        void OnEnter(IStateMachine<T> owner);

        /// <summary>
        /// Called when the state is exited.
        /// </summary>
        /// <param name="owner">The state machine that owns this state.</param>
        void OnExit(IStateMachine<T> owner);

        /// <summary>
        /// Called every frame while the state is active.
        /// </summary>
        /// <param name="owner">The state machine that owns this state.</param>
        void OnUpdate(IStateMachine<T> owner);

        /// <summary>
        /// Called every fixed framerate frame while the state is active.
        /// </summary>
        /// <param name="owner">The state machine that owns this state.</param>
        void OnFixedUpdate(IStateMachine<T> owner);
    }
}
