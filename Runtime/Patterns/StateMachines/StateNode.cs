using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Abstract base class for a state node.
    /// Provides empty virtual implementations for IState methods.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public abstract class StateNode<T> : IState<T> where T : struct, Enum
    {
        /// <inheritdoc />
        public virtual void OnEnter(IStateMachine<T> owner) { }

        /// <inheritdoc />
        public virtual void OnExit(IStateMachine<T> owner) { }

        /// <inheritdoc />
        public virtual void OnUpdate(IStateMachine<T> owner) { }

        /// <inheritdoc />
        public virtual void OnFixedUpdate(IStateMachine<T> owner) { }
    }
}
