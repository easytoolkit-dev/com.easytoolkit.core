using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// A chainable state implementation that uses events for state lifecycle hooks.
    /// </summary>
    public class ChainableState : StateNode
    {
        /// <summary>
        /// Event raised when the state is entered.
        /// </summary>
        /// <remarks>
        /// This event is triggered after the state machine transitions to this state.
        /// Use this event to initialize state-specific logic or resources.
        /// </remarks>
        public event Action<StateMachine> Entered;

        /// <summary>
        /// Event raised when the state is exited.
        /// </summary>
        /// <remarks>
        /// This event is triggered before the state machine transitions away from this state.
        /// Use this event to clean up state-specific resources or save state data.
        /// </remarks>
        public event Action<StateMachine> Exited;

        /// <summary>
        /// Event raised every frame while the state is active.
        /// </summary>
        /// <remarks>
        /// This event is triggered during the state machine's <see cref="StateMachine.Update"/> call.
        /// Use this event for per-frame game logic that should run while this state is active.
        /// </remarks>
        public event Action<StateMachine> Updated;

        /// <summary>
        /// Event raised every fixed framerate frame while the state is active.
        /// </summary>
        /// <remarks>
        /// This event is triggered during the state machine's <see cref="StateMachine.FixedUpdate"/> call.
        /// Use this event for physics-based logic that should run while this state is active.
        /// </remarks>
        public event Action<StateMachine> FixedUpdated;

        /// <inheritdoc/>
        public override void OnEnter(StateMachine owner)
        {
            Entered?.Invoke(owner);
        }

        /// <inheritdoc/>
        public override void OnExit(StateMachine owner)
        {
            Exited?.Invoke(owner);
        }

        /// <inheritdoc/>
        public override void OnUpdate(StateMachine owner)
        {
            Updated?.Invoke(owner);
        }

        /// <inheritdoc/>
        public override void OnFixedUpdate(StateMachine owner)
        {
            FixedUpdated?.Invoke(owner);
        }
    }


    /// <summary>
    /// A chainable state implementation that uses events for state lifecycle hooks.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public class ChainableState<T> : StateNode<T> where T : struct, Enum
    {
        /// <summary>
        /// Event raised when the state is entered.
        /// </summary>
        /// <remarks>
        /// This event is triggered after the state machine transitions to this state.
        /// Use this event to initialize state-specific logic or resources.
        /// </remarks>
        public event Action<StateMachine<T>> Entered;

        /// <summary>
        /// Event raised when the state is exited.
        /// </summary>
        /// <remarks>
        /// This event is triggered before the state machine transitions away from this state.
        /// Use this event to clean up state-specific resources or save state data.
        /// </remarks>
        public event Action<StateMachine<T>> Exited;

        /// <summary>
        /// Event raised every frame while the state is active.
        /// </summary>
        /// <remarks>
        /// This event is triggered during the state machine's <see cref="StateMachine{T}.Update"/> call.
        /// Use this event for per-frame game logic that should run while this state is active.
        /// </remarks>
        public event Action<StateMachine<T>> Updated;

        /// <summary>
        /// Event raised every fixed framerate frame while the state is active.
        /// </summary>
        /// <remarks>
        /// This event is triggered during the state machine's <see cref="StateMachine{T}.FixedUpdate"/> call.
        /// Use this event for physics-based logic that should run while this state is active.
        /// </remarks>
        public event Action<StateMachine<T>> FixedUpdated;

        /// <inheritdoc/>
        public override void OnEnter(StateMachine<T> owner)
        {
            Entered?.Invoke(owner);
        }

        /// <inheritdoc/>
        public override void OnExit(StateMachine<T> owner)
        {
            Exited?.Invoke(owner);
        }

        /// <inheritdoc/>
        public override void OnUpdate(StateMachine<T> owner)
        {
            Updated?.Invoke(owner);
        }

        /// <inheritdoc/>
        public override void OnFixedUpdate(StateMachine<T> owner)
        {
            FixedUpdated?.Invoke(owner);
        }
    }
}
