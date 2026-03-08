using System;

namespace EasyToolkit.Core.Patterns.Implementations
{
    /// <summary>
    /// A chainable state implementation that uses events for state lifecycle hooks.
    /// </summary>
    public class ChainableState : IChainableState
    {
        /// <inheritdoc/>
        public event Action<IStateMachine> Entered;

        /// <inheritdoc/>
        public event Action<IStateMachine> Exited;

        /// <inheritdoc/>
        public event Action<IStateMachine> Updated;

        /// <inheritdoc/>
        public event Action<IStateMachine> FixedUpdated;

        /// <inheritdoc/>
        void IState.OnEnter(IStateMachine owner)
        {
            Entered?.Invoke(owner);
        }

        /// <inheritdoc/>
        void IState.OnExit(IStateMachine owner)
        {
            Exited?.Invoke(owner);
        }

        /// <inheritdoc/>
        void IState.OnUpdate(IStateMachine owner)
        {
            Updated?.Invoke(owner);
        }

        /// <inheritdoc/>
        void IState.OnFixedUpdate(IStateMachine owner)
        {
            FixedUpdated?.Invoke(owner);
        }
    }
}
