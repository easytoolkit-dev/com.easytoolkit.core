using System;

namespace EasyToolkit.Core.Patterns.Implementations
{
    /// <summary>
    /// A chainable state implementation that uses events for state lifecycle hooks.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public class ChainableState<T> : IChainableState<T> where T : struct, Enum
    {
        /// <inheritdoc/>
        public event Action<IStateMachine<T>> Entered;

        /// <inheritdoc/>
        public event Action<IStateMachine<T>> Exited;

        /// <inheritdoc/>
        public event Action<IStateMachine<T>> Updated;

        /// <inheritdoc/>
        public event Action<IStateMachine<T>> FixedUpdated;

        /// <inheritdoc/>
        void IState<T>.OnEnter(IStateMachine<T> owner)
        {
            Entered?.Invoke(owner);
        }

        /// <inheritdoc/>
        void IState<T>.OnExit(IStateMachine<T> owner)
        {
            Exited?.Invoke(owner);
        }

        /// <inheritdoc/>
        void IState<T>.OnUpdate(IStateMachine<T> owner)
        {
            Updated?.Invoke(owner);
        }

        /// <inheritdoc/>
        void IState<T>.OnFixedUpdate(IStateMachine<T> owner)
        {
            FixedUpdated?.Invoke(owner);
        }
    }
}
