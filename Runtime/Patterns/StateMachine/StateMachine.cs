using System;
using System.Collections.Generic;
using EasyToolkit.Core.Reflection;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// A State Machine implementation where states are identified by string keys.
    /// </summary>
    /// <remarks>
    /// This implementation enforces strict state validation - all states must be registered via
    /// <see cref="AddState"/> before they can be used. For a lenient implementation that allows
    /// transitions to non-existent states, use <see cref="LenientStateMachine"/>.
    /// </remarks>
    public class StateMachine : IStateView, IStateMachineTickable, IStateMachineFixedTickable
    {
        // Dictionary for O(1) state lookup during transitions
        private readonly Dictionary<string, StateNode> _stateByKey = new();

        /// <inheritdoc/>
        public event StateChangeHandler StateChanged;

        /// <inheritdoc/>
        public StateNode CurrentState { get; protected set; }

        /// <inheritdoc/>
        public string CurrentStateKey { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine"/> class.
        /// </summary>
        public StateMachine()
        {
        }

        /// <summary>
        /// Adds a state to the state machine.
        /// </summary>
        /// <param name="keyName">The key name for the state.</param>
        /// <param name="state">The state instance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when a state with the same key already exists in the state machine.
        /// </exception>
        public void AddState(string keyName, StateNode state)
        {
            if (state.GetType().IsImplementsGenericDefinition(typeof(StateNode<>)))
            {
                throw new ArgumentException(
                    "Cannot add a typed state to a non-generic StateMachine. " +
                    "The state implements IState<T> but this StateMachine uses string keys. " +
                    "Use StateMachine<T> with the appropriate enum type instead.",
                    nameof(state));
            }

            if (!_stateByKey.TryAdd(keyName, state))
            {
                throw new ArgumentException(
                    $"State '{keyName}' already exists in StateMachine. Cannot add duplicate states.", nameof(keyName));
            }
        }

        /// <summary>
        /// Removes a state from the state machine.
        /// </summary>
        /// <param name="keyName">The key name of the state to remove.</param>
        public void RemoveState(string keyName)
        {
            if (_stateByKey.ContainsKey(keyName))
            {
                _stateByKey.Remove(keyName);
            }
        }

        /// <inheritdoc/>
        public StateNode FindState(string keyName)
        {
            return _stateByKey.GetValueOrDefault(keyName);
        }

        /// <summary>
        /// Starts the state machine with the specified state.
        /// Can only be called when the state machine has not been started yet.
        /// </summary>
        /// <param name="keyName">The key name of the starting state.</param>
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
                throw new InvalidOperationException(
                    $"StateMachine is already running (CurrentState: '{CurrentStateKey}'). Use ChangeState to transition.");
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
                throw new KeyNotFoundException(
                    $"State '{keyName}' does not exist in StateMachine! Ensure the state is added before starting.");
            }
        }

        /// <summary>
        /// Changes the current state to the state with the specified key.
        /// </summary>
        /// <param name="keyName">The key name of the target state.</param>
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
                throw new KeyNotFoundException(
                    $"State '{keyName}' does not exist in StateMachine! Ensure the state is added before transitioning to it.");
            }
        }

        /// <inheritdoc/>
        void IStateMachineTickable.OnTick(float deltaTime)
        {
            CurrentState?.OnTick(this, deltaTime);
        }

        /// <inheritdoc/>
        void IStateMachineFixedTickable.OnFixedTick(float deltaTime)
        {
            CurrentState?.OnFixedTick(this, deltaTime);
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

    /// <summary>
    /// A generic State Machine implementation where states are identified by an enum type.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    /// <remarks>
    /// This implementation enforces strict state validation - all states must be registered via
    /// <see cref="AddState"/> before they can be used. For a lenient implementation that allows
    /// transitions to non-existent states, use <see cref="LenientStateMachine{T}"/>.
    /// </remarks>
    public class StateMachine<T> : IStateView<T>, IStateMachineTickable, IStateMachineFixedTickable
        where T : struct, Enum
    {
        private readonly Dictionary<T, StateNode<T>> _stateByKey = new();
        private StateChangeHandler _untypedStateChangeHandler;

        /// <inheritdoc/>
        public event StateChangeHandler<T> StateChanged;

        /// <inheritdoc/>
        public StateNode<T> CurrentState { get; protected set; }

        /// <inheritdoc/>
        public T? CurrentStateKey { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine{T}"/> class.
        /// </summary>
        public StateMachine()
        {
        }

        /// <summary>
        /// Adds a state to the state machine.
        /// </summary>
        /// <param name="key">The enum key for the state.</param>
        /// <param name="state">The state instance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when a state with the same key already exists in the state machine.
        /// </exception>
        public void AddState(T key, StateNode<T> state)
        {
            if (!_stateByKey.TryAdd(key, state))
            {
                throw new ArgumentException($"State {key} already exists in StateMachine. Cannot add duplicate states.",
                    nameof(key));
            }
        }

        /// <summary>
        /// Removes a state from the state machine.
        /// </summary>
        /// <param name="key">The enum key of the state to remove.</param>
        public void RemoveState(T key)
        {
            if (_stateByKey.ContainsKey(key))
            {
                _stateByKey.Remove(key);
            }
        }

        public StateNode<T> FindState(T key)
        {
            return _stateByKey.GetValueOrDefault(key);
        }

        /// <summary>
        /// Starts the state machine with the specified state.
        /// Can only be called when the state machine has not been started yet.
        /// </summary>
        /// <param name="key">The enum key of the starting state.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the state machine has already been started.
        /// Use <see cref="ChangeState"/> to transition to a different state.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the state key does not exist.
        /// </exception>
        public virtual void StartState(T key)
        {
            if (CurrentState != null)
            {
                throw new InvalidOperationException(
                    $"StateMachine is already running (CurrentState: {CurrentStateKey}). Use ChangeState to transition.");
            }

            if (_stateByKey.TryGetValue(key, out var newState))
            {
                CurrentState = newState;
                CurrentStateKey = key;
                CurrentState.OnEnter(this);

                OnStateChanged(null, key);
            }
            else
            {
                throw new KeyNotFoundException(
                    $"State {key} does not exist in StateMachine! Ensure the state is added before starting.");
            }
        }

        /// <summary>
        /// Changes the current state to the state with the specified key.
        /// </summary>
        /// <param name="key">The enum key of the target state.</param>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the target state key does not exist in the state machine.
        /// </exception>
        public virtual void ChangeState(T key)
        {
            if (_stateByKey.TryGetValue(key, out var newState))
            {
                var previousKey = CurrentStateKey;

                if (CurrentState != null)
                {
                    CurrentState.OnExit(this);
                }

                CurrentState = newState;
                CurrentStateKey = key;
                CurrentState.OnEnter(this);

                OnStateChanged(previousKey, CurrentStateKey.Value);
            }
            else
            {
                throw new KeyNotFoundException(
                    $"State {key} does not exist in StateMachine! Ensure the state is added before transitioning to it.");
            }
        }

        /// <inheritdoc/>
        void IStateMachineTickable.OnTick(float deltaTime)
        {
            CurrentState?.OnTick(this, deltaTime);
        }

        /// <inheritdoc/>
        void IStateMachineFixedTickable.OnFixedTick(float deltaTime)
        {
            CurrentState?.OnFixedTick(this, deltaTime);
        }

        /// <summary>
        /// Raises the <see cref="StateChanged"/> event with the specified state transition.
        /// </summary>
        /// <param name="previousStateKey">The previous state key.</param>
        /// <param name="currentStateKey">The current state key.</param>
        protected virtual void OnStateChanged(T? previousStateKey, T currentStateKey)
        {
            StateChanged?.Invoke(previousStateKey, currentStateKey);
            _untypedStateChangeHandler?.Invoke(previousStateKey?.ToString(), currentStateKey.ToString());
        }
    }
}
