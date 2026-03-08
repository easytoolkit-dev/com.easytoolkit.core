using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// A chainable state implementation that allows fluent configuration of state callbacks.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public class ChainableState<T> : IState<T> where T : struct, Enum
    {
        private Action<IStateMachine<T>> _onEnter;
        private Action<IStateMachine<T>> _onExit;
        private Action<IStateMachine<T>> _onUpdate;
        private Action<IStateMachine<T>> _onFixedUpdate;

        /// <summary>
        /// Sets the callback to be invoked when the state is entered.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        /// <returns>The current instance for method chaining.</returns>
        public ChainableState<T> OnEnter(Action<IStateMachine<T>> callback)
        {
            _onEnter = callback;
            return this;
        }

        /// <summary>
        /// Sets the callback to be invoked when the state is exited.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        /// <returns>The current instance for method chaining.</returns>
        public ChainableState<T> OnExit(Action<IStateMachine<T>> callback)
        {
            _onExit = callback;
            return this;
        }

        /// <summary>
        /// Sets the callback to be invoked every frame while the state is active.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        /// <returns>The current instance for method chaining.</returns>
        public ChainableState<T> OnUpdate(Action<IStateMachine<T>> callback)
        {
            _onUpdate = callback;
            return this;
        }

        /// <summary>
        /// Sets the callback to be invoked every fixed framerate frame while the state is active.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        /// <returns>The current instance for method chaining.</returns>
        public ChainableState<T> OnFixedUpdate(Action<IStateMachine<T>> callback)
        {
            _onFixedUpdate = callback;
            return this;
        }

        void IState<T>.OnEnter(IStateMachine<T> owner)
        {
            _onEnter?.Invoke(owner);
        }

        void IState<T>.OnExit(IStateMachine<T> owner)
        {
            _onExit?.Invoke(owner);
        }

        void IState<T>.OnUpdate(IStateMachine<T> owner)
        {
            _onUpdate?.Invoke(owner);
        }

        void IState<T>.OnFixedUpdate(IStateMachine<T> owner)
        {
            _onFixedUpdate?.Invoke(owner);
        }
    }
}
