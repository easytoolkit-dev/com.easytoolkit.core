using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Abstract base class for a state node.
    /// Provides empty virtual implementations for IState methods.
    /// </summary>
    public abstract class StateNode : IState
    {
        /// <inheritdoc />
        public virtual void OnEnter(IStateMachine owner)
        {
        }

        /// <inheritdoc />
        public virtual void OnExit(IStateMachine owner)
        {
        }

        /// <inheritdoc />
        public virtual void OnUpdate(IStateMachine owner)
        {
        }

        /// <inheritdoc />
        public virtual void OnFixedUpdate(IStateMachine owner)
        {
        }
    }
}
