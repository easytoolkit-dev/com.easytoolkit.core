using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// A lenient State Machine implementation that allows transitions to non-existent states.
    /// </summary>
    /// <remarks>
    /// Unlike the strict <see cref="StateMachine"/>, this implementation allows state transitions
    /// to keys that haven't been registered. When transitioning to a missing state, the state machine
    /// updates its current state key and triggers the <see cref="StateChanged"/> event
    /// without calling state lifecycle methods.
    ///
    /// This is useful for scenarios where states may be dynamically added or where graceful degradation
    /// is preferred over throwing exceptions.
    /// </remarks>
    public class LenientStateMachine : StateMachine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LenientStateMachine"/> class.
        /// </summary>
        public LenientStateMachine()
        {
        }

        /// <inheritdoc/>
        /// <remarks>
        /// This override allows starting the state machine with a non-existent state key.
        /// When the state doesn't exist, <see cref="StateMachine.CurrentState"/> will be null and
        /// <see cref="StateMachine.CurrentStateKey"/> will be set to the specified key.
        /// </remarks>
        public override void StartState(string keyName)
        {
            if (CurrentState != null)
            {
                throw new InvalidOperationException(
                    $"StateMachine is already running (CurrentState: '{CurrentStateKey}'). Use ChangeState to transition.");
            }

            var newState = FindState(keyName);
            if (newState != null)
            {
                CurrentState = newState;
                CurrentStateKey = keyName;
                CurrentState.OnEnter(this);
                OnStateChanged(null, keyName);
            }
            else
            {
                // Lenient mode: allow transition to missing state
                CurrentState = null;
                CurrentStateKey = keyName;
                OnStateChanged(null, keyName);
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// This override allows transitioning to a non-existent state key.
        /// When the target state doesn't exist, <see cref="StateMachine.CurrentState"/> will be set to null.
        /// </remarks>
        public override void ChangeState(string keyName)
        {
            var newState = FindState(keyName);
            if (newState != null)
            {
                var previousKey = CurrentStateKey;
                CurrentState?.OnExit(this);
                CurrentState = newState;
                CurrentStateKey = keyName;
                CurrentState.OnEnter(this);
                OnStateChanged(previousKey, CurrentStateKey);
            }
            else
            {
                // Lenient mode: allow transition to missing state
                var previousKey = CurrentStateKey;
                CurrentState?.OnExit(this);
                CurrentState = null;
                CurrentStateKey = keyName;
                OnStateChanged(previousKey, CurrentStateKey);
            }
        }
    }


    /// <summary>
    /// A lenient State Machine implementation that allows transitions to non-existent states.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    /// <remarks>
    /// Unlike the strict <see cref="StateMachine{T}"/>, this implementation allows state transitions
    /// to keys that haven't been registered. When transitioning to a missing state, the state machine
    /// updates its current state key and triggers the <see cref="StateMachine{T}.StateChanged"/> event
    /// without calling state lifecycle methods.
    ///
    /// This is useful for scenarios where states may be dynamically added or where graceful degradation
    /// is preferred over throwing exceptions.
    /// </remarks>
    public class LenientStateMachine<T> : StateMachine<T> where T : struct, Enum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LenientStateMachine{T}"/> class.
        /// </summary>
        public LenientStateMachine()
        {
        }

        /// <inheritdoc/>
        /// <remarks>
        /// This override allows starting the state machine with a non-existent state key.
        /// When the state doesn't exist, <see cref="StateMachine{T}.CurrentState"/> will be null and
        /// <see cref="StateMachine{T}.CurrentStateKey"/> will be set to the specified key.
        /// </remarks>
        public override void StartState(T key)
        {
            if (CurrentState != null)
            {
                throw new InvalidOperationException(
                    $"StateMachine is already running (CurrentState: {CurrentStateKey}). Use ChangeState to transition.");
            }

            var newState = FindState(key);
            if (newState != null)
            {
                CurrentState = newState;
                CurrentStateKey = key;
                newState.OnEnter(this);
                OnStateChanged(null, key);
            }
            else
            {
                // Lenient mode: allow transition to missing state
                CurrentState = null;
                CurrentStateKey = key;
                OnStateChanged(null, key);
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// This override allows transitioning to a non-existent state key.
        /// When the target state doesn't exist, <see cref="StateMachine{T}.CurrentState"/> will be set to null.
        /// </remarks>
        public override void ChangeState(T key)
        {
            var newState = FindState(key);
            if (newState != null)
            {
                var previousKey = CurrentStateKey;
                CurrentState?.OnExit(this);
                CurrentState = newState;
                CurrentStateKey = key;
                newState.OnEnter(this);
                OnStateChanged(previousKey, CurrentStateKey.Value);
            }
            else
            {
                // Lenient mode: allow transition to missing state
                var previousKey = CurrentStateKey;
                CurrentState?.OnExit(this);
                CurrentState = null;
                CurrentStateKey = key;
                OnStateChanged(previousKey, CurrentStateKey.Value);
            }
        }
    }
}
