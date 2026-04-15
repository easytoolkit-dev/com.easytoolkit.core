using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Abstract base class for a state node.
    /// Provides empty virtual implementations for IState methods.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public abstract class StateNode<T> : IState<T>, IState where T : struct, Enum
    {
        /// <inheritdoc />
        public virtual void OnEnter(IStateMachine<T> owner)
        {
        }

        /// <inheritdoc />
        public virtual void OnExit(IStateMachine<T> owner)
        {
        }

        /// <inheritdoc />
        public virtual void OnUpdate(IStateMachine<T> owner)
        {
        }

        /// <inheritdoc />
        public virtual void OnFixedUpdate(IStateMachine<T> owner)
        {
        }

        void IState.OnEnter(IStateMachine owner)
        {
            OnEnter(ValidateOwner(owner));
        }

        void IState.OnExit(IStateMachine owner)
        {
            OnExit(ValidateOwner(owner));
        }

        void IState.OnUpdate(IStateMachine owner)
        {
            OnUpdate(ValidateOwner(owner));
        }

        void IState.OnFixedUpdate(IStateMachine owner)
        {
            OnFixedUpdate(ValidateOwner(owner));
        }

        private IStateMachine<T> ValidateOwner(IStateMachine owner)
        {
            if (owner is not IStateMachine<T> castedOwner)
            {
                throw new ArgumentException(
                    $"State machine must implement IStateMachine<{typeof(T).Name}>. " +
                    $"This state node is typed for {typeof(T).Name} but the provided state machine " +
                    "does not match. Ensure you are using a StateMachine<T> with the correct enum type.",
                    nameof(owner));
            }

            return castedOwner;
        }
    }
}
