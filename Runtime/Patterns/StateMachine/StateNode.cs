using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Abstract base class for a state node.
    /// Provides empty virtual implementations for IState methods.
    /// </summary>
    public abstract class StateNode
    {
        /// <summary>
        /// Called when the state is entered.
        /// </summary>
        /// <param name="owner">The state machine that owns this state.</param>
        public virtual void OnEnter(StateMachine owner)
        {
        }

        /// <summary>
        /// Called when the state is exited.
        /// </summary>
        /// <param name="owner">The state machine that owns this state.</param>
        public virtual void OnExit(StateMachine owner)
        {
        }

        /// <summary>
        /// Called every frame while the state is active.
        /// </summary>
        /// <param name="owner">The state machine that owns this state.</param>
        public virtual void OnTick(StateMachine owner, float deltaTime)
        {
        }

        /// <summary>
        /// Called every fixed framerate frame while the state is active.
        /// </summary>
        /// <param name="owner">The state machine that owns this state.</param>
        public virtual void OnFixedTick(StateMachine owner, float deltaTime)
        {
        }
    }

    /// <summary>
    /// Abstract base class for a state node.
    /// Provides empty virtual implementations for IState methods.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public abstract class StateNode<T> where T : struct, Enum
    {
        /// <summary>
        /// Called when the state is entered.
        /// </summary>
        /// <param name="owner">The state machine that owns this state.</param>
        public virtual void OnEnter(StateMachine<T> owner)
        {
        }

        /// <summary>
        /// Called when the state is exited.
        /// </summary>
        /// <param name="owner">The state machine that owns this state.</param>
        public virtual void OnExit(StateMachine<T> owner)
        {
        }

        /// <summary>
        /// Called every frame while the state is active.
        /// </summary>
        /// <param name="owner">The state machine that owns this state.</param>
        public virtual void OnTick(StateMachine<T> owner, float deltaTime)
        {
        }

        /// <summary>
        /// Called every fixed framerate frame while the state is active.
        /// </summary>
        /// <param name="owner">The state machine that owns this state.</param>
        public virtual void OnFixedTick(StateMachine<T> owner, float deltaTime)
        {
        }
    }
}
