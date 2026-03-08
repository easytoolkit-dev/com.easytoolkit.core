using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Interface for a chainable state that uses event-based callbacks for state lifecycle hooks.
    /// </summary>
    /// <seealso cref="ChainableStateExtensions"/>
    public interface IChainableState : IState
    {
        /// <summary>
        /// Event raised when the state is entered.
        /// </summary>
        /// <remarks>
        /// This event is triggered after the state machine transitions to this state.
        /// Use this event to initialize state-specific logic or resources.
        /// </remarks>
        event Action<IStateMachine> Entered;

        /// <summary>
        /// Event raised when the state is exited.
        /// </summary>
        /// <remarks>
        /// This event is triggered before the state machine transitions away from this state.
        /// Use this event to clean up state-specific resources or save state data.
        /// </remarks>
        event Action<IStateMachine> Exited;

        /// <summary>
        /// Event raised every frame while the state is active.
        /// </summary>
        /// <remarks>
        /// This event is triggered during the state machine's <see cref="IStateMachine.Update"/> call.
        /// Use this event for per-frame game logic that should run while this state is active.
        /// </remarks>
        event Action<IStateMachine> Updated;

        /// <summary>
        /// Event raised every fixed framerate frame while the state is active.
        /// </summary>
        /// <remarks>
        /// This event is triggered during the state machine's <see cref="IStateMachine.FixedUpdate"/> call.
        /// Use this event for physics-based logic that should run while this state is active.
        /// </remarks>
        event Action<IStateMachine> FixedUpdated;
    }
}
