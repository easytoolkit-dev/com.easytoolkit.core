using System;

namespace EasyToolkit.Core.Patterns.StateMachines
{
    /// <summary>
    /// Abstract base class for a state node.
    /// Provides empty virtual implementations for IState methods.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public abstract class StateNode<T> : IState<T> where T : Enum
    {
        /// <inheritdoc />
        public virtual void OnEnter() { }

        /// <inheritdoc />
        public virtual void OnExit() { }

        /// <inheritdoc />
        public virtual void OnUpdate() { }

        /// <inheritdoc />
        public virtual void OnFixedUpdate() { }
    }
}
