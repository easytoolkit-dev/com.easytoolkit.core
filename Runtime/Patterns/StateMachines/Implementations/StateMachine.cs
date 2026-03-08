using System;
using System.Collections.Generic;
using EasyToolkit.Core.Reflection;

namespace EasyToolkit.Core.Patterns.Implementations
{
    /// <summary>
    /// A State Machine implementation where states are identified by string keys.
    /// </summary>
    /// <remarks>
    /// This implementation enforces strict state validation - all states must be registered via
    /// <see cref="AddState"/> before they can be used. For a lenient implementation that allows
    /// transitions to non-existent states, use <see cref="LenientStateMachine"/>.
    /// </remarks>
    public class StateMachine : IStateMachine
    {
        // Dictionary for O(1) state lookup during transitions
        private readonly Dictionary<string, IState> _stateByKey = new();

        /// <inheritdoc/>
        public event StateChangeHandler StateChanged;

        /// <inheritdoc/>
        public IState CurrentState { get; protected set; }

        /// <inheritdoc/>
        public string CurrentStateKey { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine"/> class.
        /// </summary>
        public StateMachine()
        {
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">
        /// Thrown when a state with the same key already exists in the state machine.
        /// </exception>
        public void AddState(string keyName, IState state)
        {
            if (state.GetType().IsImplementsGenericDefinition(typeof(IState<>)))
            {
                throw new ArgumentException(
                    "Cannot add a typed state to a non-generic StateMachine. " +
                    "The state implements IState<T> but this StateMachine uses string keys. " +
                    "Use StateMachine<T> with the appropriate enum type instead.",
                    nameof(state));
            }

            if (!_stateByKey.TryAdd(keyName, state))
            {
                throw new ArgumentException($"State '{keyName}' already exists in StateMachine. Cannot add duplicate states.", nameof(keyName));
            }
        }

        /// <inheritdoc/>
        public void RemoveState(string keyName)
        {
            if (_stateByKey.ContainsKey(keyName))
            {
                _stateByKey.Remove(keyName);
            }
        }

        /// <inheritdoc/>
        public IState FindState(string keyName)
        {
            return _stateByKey.GetValueOrDefault(keyName);
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the state machine has already been started.
        /// Use <see cref="ChangeState"/> to transition to a different state.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the state key does not exist.
        /// </exception>
        public virtual void StartState(string keyName)
        {
            if (CurrentState != null)
            {
                throw new InvalidOperationException($"StateMachine is already running (CurrentState: '{CurrentStateKey}'). Use ChangeState to transition.");
            }

            if (_stateByKey.TryGetValue(keyName, out var newState))
            {
                CurrentState = newState;
                CurrentStateKey = keyName;
                CurrentState.OnEnter(this);

                StateChanged?.Invoke(null, keyName);
            }
            else
            {
                throw new KeyNotFoundException($"State '{keyName}' does not exist in StateMachine! Ensure the state is added before starting.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the target state key does not exist in the state machine.
        /// </exception>
        public virtual void ChangeState(string keyName)
        {
            if (_stateByKey.TryGetValue(keyName, out var newState))
            {
                var previousKey = CurrentStateKey;

                if (CurrentState != null)
                {
                    CurrentState.OnExit(this);
                }

                CurrentState = newState;
                CurrentStateKey = keyName;
                CurrentState.OnEnter(this);

                StateChanged?.Invoke(previousKey, CurrentStateKey);
            }
            else
            {
                throw new KeyNotFoundException($"State '{keyName}' does not exist in StateMachine! Ensure the state is added before transitioning to it.");
            }
        }

        /// <inheritdoc/>
        public void Update()
        {
            CurrentState?.OnUpdate(this);
        }

        /// <inheritdoc/>
        public void FixedUpdate()
        {
            CurrentState?.OnFixedUpdate(this);
        }

        /// <summary>
        /// Raises the <see cref="StateChanged"/> event with the specified state transition.
        /// </summary>
        /// <param name="previousStateKeyName">The previous state key name.</param>
        /// <param name="currentStateKeyName">The current state key name.</param>
        protected virtual void OnStateChanged(string previousStateKeyName, string currentStateKeyName)
        {
            StateChanged?.Invoke(previousStateKeyName, currentStateKeyName);
        }
    }
}
