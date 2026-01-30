using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// A chainable state implementation that allows fluent configuration of state callbacks.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public class ChainableState<T> : IState<T> where T : Enum
    {
        private Action _onEnter;
        private Action _onExit;
        private Action _onUpdate;
        private Action _onFixedUpdate;

        /// <summary>
        /// Sets the callback to be invoked when the state is entered.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        /// <returns>The current instance for method chaining.</returns>
        public ChainableState<T> OnEnter(Action callback)
        {
            _onEnter = callback;
            return this;
        }

        /// <summary>
        /// Sets the callback to be invoked when the state is exited.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        /// <returns>The current instance for method chaining.</returns>
        public ChainableState<T> OnExit(Action callback)
        {
            _onExit = callback;
            return this;
        }

        /// <summary>
        /// Sets the callback to be invoked every frame while the state is active.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        /// <returns>The current instance for method chaining.</returns>
        public ChainableState<T> OnUpdate(Action callback)
        {
            _onUpdate = callback;
            return this;
        }

        /// <summary>
        /// Sets the callback to be invoked every fixed framerate frame while the state is active.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        /// <returns>The current instance for method chaining.</returns>
        public ChainableState<T> OnFixedUpdate(Action callback)
        {
            _onFixedUpdate = callback;
            return this;
        }

        void IState<T>.OnEnter()
        {
            _onEnter?.Invoke();
        }

        void IState<T>.OnExit()
        {
            _onExit?.Invoke();
        }

        void IState<T>.OnUpdate()
        {
            _onUpdate?.Invoke();
        }

        void IState<T>.OnFixedUpdate()
        {
            _onFixedUpdate?.Invoke();
        }
    }
}
